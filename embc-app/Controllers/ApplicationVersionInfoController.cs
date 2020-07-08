using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class ApplicationVersionInfoController : Controller
    {
        private readonly IConfiguration Configuration;        
        
        public ApplicationVersionInfoController(IConfiguration configuration)
        {
            Configuration = configuration;                  
        }

        /// <summary>
        /// Return the version of the running application
        /// </summary>
        /// <returns>The version of the running application</returns>
        [HttpGet]        
        public ActionResult GetApplicationVersionInfo()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            DateTime creationTime = System.IO.File.GetLastWriteTimeUtc(assembly.Location);
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = $"{Configuration.GetReleaseVersion()}{fvi.FileVersion}";
            var versionNumbers = fvi.FileVersion.Split('.');
            string patchNum = "";
            string minorNums = "";
            // Now just try to error out on me, Openshift. I'll catch you.
            try
            {
                patchNum = versionNumbers[1];
            }
            catch(Exception)
            {
                patchNum = "(index 1 out of range)";
            }
            try
            {
                minorNums = versionNumbers[2];
            }
            catch(Exception)
            {
                minorNums = "(index 2 out of range)";
            }
            
            string fileVersion2 = $"{Configuration.GetReleaseVersion()}{patchNum}";
            string fileVersion3 = $"{Configuration.GetReleaseVersion()}{minorNums}";
            string fileVersion4 = $"{Configuration.GetReleaseVersion()}{patchNum}.{minorNums}";
            
            ApplicationVersionInfo avi = new ApplicationVersionInfo()
            {
                BaseUri = Configuration["BASE_URI"],
                BasePath = Configuration["BASE_PATH"],
                Environment = Configuration["ASPNETCORE_ENVIRONMENT"],                
                SourceCommit = Configuration["OPENSHIFT_BUILD_COMMIT"],
                SourceRepository = Configuration["OPENSHIFT_BUILD_SOURCE"],
                SourceReference = Configuration["OPENSHIFT_BUILD_REFERENCE"],
                FileCreationTime = creationTime.ToString("O"), // Use the round trip format as it includes the time zone.
                FileVersion = fileVersion,
                FileVersion2 = fileVersion2,
                FileVersion3 = fileVersion3,
                FileVersion4 = fileVersion4,
        };

            return Json(avi);
        }
    
	}
}
