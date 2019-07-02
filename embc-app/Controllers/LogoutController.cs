using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment env;

        public LogoutController(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            string logoutPath = Configuration.GetSiteMinderLogoutUrl();
            return Redirect(logoutPath + $"?returl={Configuration.GetBaseUri()}{Configuration.GetBasePath()}&retnow=1");
        }
    }
}
