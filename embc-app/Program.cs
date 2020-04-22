using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using System;
using System.Net.Http;

namespace Gov.Jag.Embc.Public
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().Enrich.FromLogContext().CreateLogger();
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseHealthChecks("/hc")
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.WithMachineName()
                        .Enrich.WithProcessId()
                        .Enrich.WithProcessName()
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                        .WriteTo.Console(formatter: new RenderedCompactJsonFormatter());

                    var splunkUrl = hostingContext.Configuration.GetSplunkUrl();
                    var splunkToken = hostingContext.Configuration.GetSplunkToken();
                    if (!hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        if (string.IsNullOrWhiteSpace(splunkToken) || string.IsNullOrWhiteSpace(splunkUrl))
                        {
                            Log.Error($"Splunk logging sink is not configured properly, check SPLUNK_TOKEN and SPLUNK_URL env vars");
                        }
                        else
                        {
                            loggerConfiguration.WriteTo.EventCollector(
                                splunkHost: splunkUrl,
                                eventCollectorToken: splunkToken,
                                messageHandler: new HttpClientHandler
                                {
                                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                                },
                                renderTemplate: false);
                        }
                    }
                })
                .UseStartup<Startup>();
    }
}