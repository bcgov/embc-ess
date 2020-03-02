using AutoMapper;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Seeder;
using Gov.Jag.Embc.Public.Services;
using Gov.Jag.Embc.Public.Services.Referrals;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public
{
    public class Startup
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
            this.environment = environment;
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
                .AddMvc(opts =>
                {
                    // authorize on all controllers by default
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

            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ESS API";
                    document.Info.Description = "Emergency Management BC Evacuee Support System API";
                };
            }
            );

            // setup siteminder authentication
            if (configuration.GetAuthenticationMode() == "SM")
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                    options.DefaultChallengeScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                }).AddSiteminderAuth();
            }
            else
            {
                //services.AddSingleton<IClaimsTransformation, KeyCloakClaimTransformer>(); //this will cause the transformation to be called for every request by the middleware
                services.AddScoped<KeyCloakClaimTransformer, KeyCloakClaimTransformer>();   //this is to be able to resolve once in OnTicketReceived event only
                services.AddAuthentication(options =>
                {
                    //Default to cookie auth, challenge with OIDC
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                    //JWT bearer to support direct API authentication
                    .AddJwtBearer(options =>
                    {
                        configuration.GetSection("auth:jwt").Bind(options);

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = async c =>
                            {
                                var logger = c.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                                logger.LogError(c.Exception, $"Error authenticating JWTBearer token");
                                c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                c.Response.ContentType = "text/plain"; if (environment.IsDevelopment())
                                {
                                    // Debug only, in production do not share exceptions with the
                                    // remote host.
                                    await c.Response.WriteAsync(c.Exception.ToString());
                                }
                                await c.Response.WriteAsync("An error occurred processing yourauthentication.");
                            },

                            OnTokenValidated = async c =>
                                                        {
                                                            var claimTransformer = c.HttpContext.RequestServices.GetRequiredService<KeyCloakClaimTransformer>();
                                                            c.Principal = await claimTransformer.TransformAsync(c.Principal);
                                                            c.Success();
                                                        }
                        };
                    })
                    //cookies as default and sign in, principal will be saved as a cookie (no need to session state)
                    .AddCookie(options =>
                    {
                        configuration.GetSection("auth:cookie").Bind(options);
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.LoginPath = "/login";
                    })
                    .AddOpenIdConnect(options => //oidc authentication for challenge authentication request
                    {
                        configuration.GetSection("auth:oidc").Bind(options);
                        options.Events = new OpenIdConnectEvents()
                        {
                            OnAuthenticationFailed = async c =>
                            {
                                var logger = c.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();
                                logger.LogError(c.Exception, $"Error authenticating OIDC token");

                                c.HandleResponse();

                                c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                c.Response.ContentType = "text/plain";
                                if (environment.IsDevelopment())
                                {
                                    // Debug only, in production do not share exceptions with the
                                    // remote host.
                                    await c.Response.WriteAsync(c.Exception.ToString());
                                }
                                await c.Response.WriteAsync("An error occurred processing your authentication.");
                            },
                            OnTicketReceived = async c =>
                            {
                                var claimTransformer = c.HttpContext.RequestServices.GetRequiredService<KeyCloakClaimTransformer>();
                                c.Principal = await claimTransformer.TransformAsync(c.Principal);
                                c.Success();
                            }
                        };
                    });

                services.AddAuthorization(options =>
                {
                    //API authorization policy supports cookie or jwt authentication schemes
                    options.AddPolicy("API", policy =>
                    {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                    });

                    //Set API policy as default for [authorize] controllers
                    options.DefaultPolicy = options.GetPolicy("API");
                });
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
                        {
                            configuration.RootPath = "ClientApp/dist";
                        });

            // health checks
            services.AddHealthChecks(checks =>
            {
                checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
                checks.AddSqlCheck(configuration.GetDbName(), DatabaseTools.GetConnectionString(configuration));
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IPdfConverter, PdfConverter>();
            services.AddTransient<IReferralsService, ReferralsService>();
            services.AddTransient<IDataInterface, DataInterface>();
            // Using AddScoped rather than transient because of state. Might be incorrect.
            services.AddScoped<ICurrentUser, CurrentUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env/*, IAntiforgery antiForgery*/)
        {
            // DATABASE SETUP
            SetupDatabase(env);

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
                // no cache headers
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

            if (!env.IsProduction())
            {
                // Use NSwag for API documentation Register the Swagger generator and the Swagger UI middlewares
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
            
            var fwdHeadersOpts = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };
            fwdHeadersOpts.KnownProxies.Add(IPAddress.Parse("[::ffff:172.51.20.1]:59510"));
            
            app
                .UseForwardedHeaders(fwdHeadersOpts)
                .UseAuthentication()
                .UseCookiePolicy()
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
                        //spa.UseAngularCliServer(npmScript: "start");
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
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
            if (configuration.HasDbAdminPassword())
            {
                //For OpenShift deployments
                DatabaseTools.CreateDatabaseIfNotExists(DatabaseTools.GetSaConnectionString(configuration, "master"),
                    configuration.GetDbName(),
                    configuration.GetDbUser(),
                    configuration.GetDbUserPassword());
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
