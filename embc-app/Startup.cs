using AutoMapper;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Seeder;
using Gov.Jag.Embc.Public.Services.Referrals;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public
{
    public class Startup
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add singleton to allow Controllers to query the Request object
            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton(configuration)
                .AddDbContext<EmbcDbContext>(
                    options => options
                        .UseLoggerFactory(loggerFactory)
                        .UseSqlServer(DatabaseTools.GetConnectionString(configuration))
                        )
                // CORS policy
                .AddCors(opts =>
                {
                    opts.AddDefaultPolicy(builder =>
                    {
                        builder.WithOrigins(
                              "http://pathfinder.bcgov",
                              "https://*.pathfinder.gov.bc.ca",
                              "https://dev.justice.gov.bc.ca",
                              "https://test.justice.gov.bc.ca",
                              "https://justice.gov.bc.ca")
                              .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
                })
                //XSRF token for Angular - not working yet
                //.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN")
                .AddSession()
                .AddMvc(opts =>
                {
                    // authorize on all controllers by default
                    opts.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    // anti forgery validation by default - not working yet
                    //opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddJsonOptions(
                    opts =>
                    {
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        opts.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            // setup siteminder authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                options.DefaultChallengeScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
            }).AddSiteminderAuth();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
                        {
                            configuration.RootPath = "ClientApp/dist";
                        });

            // health checks
            services.AddHealthChecks(checks =>
            {
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
                checks.AddSqlCheck(configuration.GetValue<string>("DB_NAME"), DatabaseTools.GetConnectionString(configuration));
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IPdfConverter, PdfConverter>();
            services.AddTransient<IReferralsService, ReferralsService>();
            services.AddTransient<IDataInterface, DataInterface>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env/*, IAntiforgery antiForgery*/)
        {
            // DATABASE SETUP
            SetupDatabase(env);

            app.UsePathBase(configuration.GetValue("BASE_PATH", ""));

            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app
                // HSTS header
                .UseHsts(opts => opts.IncludeSubdomains().MaxAge(days: 365).AllResponses())
                // X-Content-Type-Options header
                .UseXContentTypeOptions()
                // Referrer-Policy header.
                .UseReferrerPolicy(opts => opts.NoReferrer())
                // X-Xss-Protection header
                .UseXXssProtection(opts => opts.EnabledWithBlockMode())
                // X-Frame-Options header
                .UseXfo(opts => opts.Deny())
                // CORS policy
                .UseCors()
                // X-xss-protection header
                .UseXXssProtection(opts => opts.EnabledWithBlockMode())
                // X-Robots-Tag header
                .UseXRobotsTag(opts => opts.NoIndex().NoFollow())
                .UseXDownloadOptions()
                // no cache  headers
                .UseNoCacheHttpHeaders()
                // CSP header
                .Use(next => context =>
                {
                    var cspHeader = configuration.CspEnabled()
                    ? "Content-Security-Policy"
                    : "Content-Security-Policy-Report-Only";

                    context.Response.Headers.Append(cspHeader, "default-src 'self' https://*.pathfinder.gov.bc.ca;" +   //captcha service
                        "script-src 'self' 'unsafe-inline' " + (env.IsDevelopment() ? "'unsafe-eval'" : "") + ";" +
                        "style-src 'self' 'unsafe-inline';" +
                        "media-src 'self' data:;" +     //captcha audio
                        "object-src 'self' blob:;" +    //referral printout pdf
                        "img-src 'self' data:;" +    //svg and images
                        "block-all-mixed-content");

                    return next(context);
                })
                // Anty forgery cookie for Angular - not working yet
                //.Use(next => context =>
                //{
                //    var path = context.Request.Path.Value;

                //    if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
                //        string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase) ||
                //        string.Equals(path, "/login", StringComparison.OrdinalIgnoreCase))
                //    {
                //        // The request token can be sent as a JavaScript-readable cookie, and Angular uses it by default.
                //        var tokens = antiForgery.GetAndStoreTokens(context);
                //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
                //    }
                //    return next(context);
                //})
                .UseStaticFiles()
                .UseSpaStaticFiles();

            app
                .UseSession()
                .UseAuthentication()
                .UseCookiePolicy(new CookiePolicyOptions
                {
                    HttpOnly = HttpOnlyPolicy.Always,
                    Secure = env.IsDevelopment()
                        ? CookieSecurePolicy.SameAsRequest
                        : CookieSecurePolicy.Always,
                    MinimumSameSitePolicy = SameSiteMode.Strict
                })

                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action=Index}/{id?}");
                })

                .UseSpa(spa =>
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

        private void SetupDatabase(IHostingEnvironment env)
        {
            var log = loggerFactory.CreateLogger<Startup>();
            log.LogInformation("Fetching the application's database context ...");

            var adminCtx = new EmbcDbContext(new DbContextOptionsBuilder<EmbcDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(DatabaseTools.GetSaConnectionString(configuration)).Options);

            if (configuration.DbFullRefresh())
            {
                log.LogWarning("DROPPING the database! ...");
                adminCtx.Database.EnsureDeleted();
            }

            log.LogInformation("Initializing the database ...");
            if (!string.IsNullOrEmpty(configuration["DB_ADMIN_PASSWORD"]))
            {
                //For OpenShift deployments
                DatabaseTools.CreateDatabaseIfNotExists(DatabaseTools.GetSaConnectionString(configuration, "master"),
                    configuration["DB_DATABASE"],
                    configuration["DB_USER"],
                    configuration["DB_PASSWORD"]);
            }

            //Check if the database exists
            var databaseExists = adminCtx.Database.CanConnect();
            if (databaseExists)
            {
                log.LogInformation("Syncing migrations prior to migrating...");
                DatabaseTools.SyncInitialMigration(DatabaseTools.GetSaConnectionString(configuration));
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
        }
    }
}
