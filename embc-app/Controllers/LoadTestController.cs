using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("websurge-allow.txt")]
    public class LoadTestController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment _env;
        private readonly SiteMinderAuthOptions _options = new SiteMinderAuthOptions();

        public LoadTestController( IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoadTest(string path)
        {
            // check to see if we have a local path.  (do not allow a redirect to another website)
            if (!_env.IsProduction())
            {
                return Ok();                
            }
            else
            {
                return NotFound();
            }
        }
    
	}
}
