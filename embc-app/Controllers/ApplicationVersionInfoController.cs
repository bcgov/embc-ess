﻿using Gov.Jag.Embc.Public.ViewModels;
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
            string fileVersion = fvi.FileVersion;
            
            ApplicationVersionInfo avi = new ApplicationVersionInfo()
            {
                BaseUri = Configuration["BASE_URI"],
                BasePath = Configuration["BASE_PATH"],
                Environment = Configuration["ASPNETCORE_ENVIRONMENT"],                
                SourceCommit = Configuration["OPENSHIFT_BUILD_COMMIT"],
                SourceRepository = Configuration["OPENSHIFT_BUILD_SOURCE"],
                SourceReference = Configuration["OPENSHIFT_BUILD_REFERENCE"],
                FileCreationTime = creationTime.ToString("O"), // Use the round trip format as it includes the time zone.
                FileVersion = fileVersion
            };

            return Json(avi);
        }
    
	}
}
