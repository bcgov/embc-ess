using AutoMapper;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Seeder;
using Gov.Jag.Embc.Public.Services.Referrals;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using NWebsec.AspNetCore.Mvc;
using NWebsec.AspNetCore.Mvc.Csp;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            // add singleton to allow Controllers to query the Request object
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<EmbcDbContext>(
                options => options
                    .UseSqlServer(DatabaseTools.GetConnectionString(Configuration)));

            // Add a memory cache
            services.AddMemoryCache();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            // for security reasons, the following headers are set.
            services.AddMvc(opts =>
            {
                // default deny
                var policy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();
                opts.Filters.Add(new AuthorizeFilter(policy));
                opts.Filters.Add(typeof(NoCacheHttpHeadersAttribute));
                opts.Filters.Add(new XRobotsTagAttribute() { NoIndex = true, NoFollow = true });
                opts.Filters.Add(typeof(XContentTypeOptionsAttribute));
                opts.Filters.Add(typeof(XDownloadOptionsAttribute));
                opts.Filters.Add(typeof(XFrameOptionsAttribute));
                opts.Filters.Add(typeof(XXssProtectionAttribute));
                //CSPReportOnly
                opts.Filters.Add(typeof(CspReportOnlyAttribute));
                opts.Filters.Add(new CspScriptSrcReportOnlyAttribute { None = true });
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(
                opts =>
                {
                    opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    opts.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;

                    // ReferenceLoopHandling is set to Ignore to prevent JSON parser issues with the
                    // user / roles model.
                    opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // setup siteminder authentication (core 2.0)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                options.DefaultChallengeScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
            }).AddSiteminderAuth(options =>
            {
            });

            // setup key ring to persist in storage.
            if (!string.IsNullOrEmpty(Configuration["KEY_RING_DIRECTORY"]))
            {
                services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Configuration["KEY_RING_DIRECTORY"]));
            }

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // health checks
            services.AddHealthChecks(checks =>
            {
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));

                //checks.AddSqlCheck(DatabaseTools.GetDatabaseName(Configuration), DatabaseTools.GetConnectionString(Configuration));
            });

            services.AddSession(opts =>
            {
                opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opts.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                opts.IdleTimeout = TimeSpan.FromMinutes(Configuration.ServerTimeoutInMinutes());
            });
            // add a data interface
            services.AddTransient<IDataInterface, DataInterface>();

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Enable the IURLHelper to be able to build links within Controllers
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IPdfConverter, PdfConverter>();
            services.AddTransient<IReferralsService, ReferralsService>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            var log = loggerFactory.CreateLogger<Startup>();

            //inject an instance of AutoMapper to the static class
            ViewModelConversions.mapper = app.ApplicationServices.GetService<IMapper>();

            // DATABASE SETUP
            log.LogInformation("Fetching the application's database context ...");

            var adminCtx = new EmbcDbContext(new DbContextOptionsBuilder<EmbcDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(DatabaseTools.GetSaConnectionString(Configuration)).Options);

            if (Configuration.DbFullRefresh())
            {
                log.LogWarning("DROPPING the database! ...");
                adminCtx.Database.EnsureDeleted();
            }

            log.LogInformation("Initializing the database ...");
            if (!string.IsNullOrEmpty(Configuration["DB_ADMIN_PASSWORD"]))
            {
                //For OpenShift deployments
                DatabaseTools.CreateDatabaseIfNotExists(DatabaseTools.GetSaConnectionString(Configuration, "master"),
                    Configuration["DB_DATABASE"],
                    Configuration["DB_USER"],
                    Configuration["DB_PASSWORD"]);
            }

            //Check if the database exists
            var databaseExists = adminCtx.Database.CanConnect();
            if (databaseExists)
            {
                log.LogInformation("Syncing migrations prior to migrating...");
                DatabaseTools.SyncInitialMigration(DatabaseTools.GetSaConnectionString(Configuration));
            }

            log.LogInformation("Migrating the database ...");
            adminCtx.Database.Migrate();

            log.LogInformation("The database migration is complete.");

            try
            {
                // run the database seeders
                log.LogInformation("Adding/Updating seed data ...");

                ISeederRepository seederRepository = new SeederRepository(adminCtx);
                var seedDataLoader = new SeedDataLoader(loggerFactory);
                var seeder = new EmbcSeeder(loggerFactory, seederRepository, env, seedDataLoader);
                seeder.SeedData();

                log.LogInformation("Seeding operations are complete.");
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("The database setup failed!");
                msg.AppendLine("The database may not be available and the application will not function as expected.");
                msg.AppendLine("Please ensure a database is available and the connection string is correct.");
                msg.AppendLine("If you are running in a development environment, ensure your test database and server configuration match the project's default connection string.");
                msg.AppendLine("Error message is " + e.Message);
                log.LogCritical(new EventId(-1, "Database Seeding Failed"), e, msg.ToString());
            }

            string pathBase = Configuration["BASE_PATH"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers.Add("Content-Security-Policy",
                                         "script-src 'self' 'unsafe-eval' 'unsafe-inline' https://apis.google.com https://maxcdn.bootstrapcdn.com https://cdnjs.cloudflare.com https://code.jquery.com https://stackpath.bootstrapcdn.com https://fonts.googleapis.com");
                ctx.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
                await next();
            });
            app.UseXContentTypeOptions();
            app.UseXfo(xfo => xfo.Deny());

            StaticFileOptions staticFileOptions = new StaticFileOptions();

            staticFileOptions.OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate, private";
                ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                ctx.Context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                ctx.Context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            };

            app.UseStaticFiles(staticFileOptions);
            app.UseSpaStaticFiles(staticFileOptions);
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseNoCacheHttpHeaders();
            // IMPORTANT: This session call MUST go before UseMvc()
            var sessionTimout = TimeSpan.FromMinutes(Configuration.ServerTimeoutInMinutes());
            app.UseSession();
            app.UseAuthentication();

            // global policy - assign here or on each controller
            // IMPORTANT: Make sure UseCors() is called BEFORE UseMvc()
            app.UseCors("AllowAnyOrigin");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core, see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                // Only run the angular CLI Server in Development mode (not staging or test.)
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
