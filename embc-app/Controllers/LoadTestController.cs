using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("websurge-allow.txt")]
    public class LoadTestController : Controller
    {
        private readonly IHostingEnvironment env;

        public LoadTestController(IHostingEnvironment env)
        {
            this.env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoadTest(string path)
        {
            // check to see if we have a local path.  (do not allow a redirect to another website)
            if (!env.IsProduction())
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
