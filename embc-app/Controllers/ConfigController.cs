using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ConfigController : Controller
    {
        private readonly IConfiguration configuration;

        public ConfigController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetApplicationVersionInfo()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;

            ConfigurationInfo avi = new ConfigurationInfo()
            {
                BaseUri = configuration["BASE_URI"],
                BasePath = configuration["BASE_PATH"],
                Environment = configuration["APP_ENVIRONMENT_TITLE"],
                SourceCommit = configuration["OPENSHIFT_BUILD_COMMIT"],
                SourceRepository = configuration["OPENSHIFT_BUILD_SOURCE"],
                SourceReference = configuration["OPENSHIFT_BUILD_REFERENCE"],
                FileCreationTime = System.IO.File.GetLastWriteTimeUtc(assembly.Location).ToString("O"), // Use the round trip format as it includes the time zone.
                FileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion,
                ClientTimeoutWarningInMinutes = configuration.UserTimeoutWarningInMinutes(),
                ClientTimeoutWarningDurationInMinutes = configuration.UserTimeoutWarningDurationInMinutes(),
                DefaultTimeoutWarningInMinutes = configuration.DefaultTimeoutWarningInMinutes(),
                DefaultWarningDurationInMinutes = configuration.DefaultTimeoutWarningDurationInMinutes(),
            };

            return Json(avi);
        }
    }
}
