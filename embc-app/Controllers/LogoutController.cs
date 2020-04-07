using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment env;
        private readonly ILogger logger;

        public LogoutController(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            this.env = env;
            logger = loggerFactory.CreateLogger(typeof(LoginController));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            logger.LogInformation($"Logout requested");

            //log out oidc and cookies
            var authenticationSchemes = new[] { OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme };
            return new SignOutResult(authenticationSchemes, new AuthenticationProperties { RedirectUri = $"{Configuration.GetBasePath()}/logout/sm" });
        }

        [HttpGet("sm")]
        [AllowAnonymous]
        public IActionResult LogoutSiteMinder()
        {
            logger.LogInformation($"Logout from SiteMinder requested");
            //log out SiteMinder and return back to logout oidc and cookies
            var logoutPath = Configuration.GetSiteMinderLogoutUrl();

            return Redirect(logoutPath + $"?returl={Configuration.GetBaseUri()}{Configuration.GetBasePath()}&retnow=1");
        }
    }
}
