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
            
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            var versionNumbers = fvi.FileVersion.Split('.');
            string minorNums = versionNumbers[2];
            string fileVersion = $"{configuration.GetReleaseVersion()}{minorNums}";

            ConfigurationInfo avi = new ConfigurationInfo()
            {
                BaseUri = configuration.GetBaseUri(),
                BasePath = configuration.GetBasePath(),
                Environment = configuration.GetEnvironmentName(),
                EnvironmentTitle = configuration.GetEnvironmentTitle(),
                SourceCommit = configuration.GetBuildCommitId(),
                SourceRepository = configuration.GetBuildSource(),
                SourceReference = configuration.GetBuildVersion(),
                FileCreationTime = System.IO.File.GetLastWriteTimeUtc(assembly.Location).ToString("O"), // Use the round trip format as it includes the time zone.
                FileVersion = fileVersion,
                ClientTimeoutWarningInMinutes = configuration.UserTimeoutWarningInMinutes(),
                ClientTimeoutWarningDurationInMinutes = configuration.UserTimeoutWarningDurationInMinutes(),
                DefaultTimeoutWarningInMinutes = configuration.DefaultTimeoutWarningInMinutes(),
                DefaultWarningDurationInMinutes = configuration.DefaultTimeoutWarningDurationInMinutes(),
            };
           
            return Json(avi);
        }
    }
}
