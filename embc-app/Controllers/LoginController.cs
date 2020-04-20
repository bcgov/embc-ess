using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private readonly ILogger logger;
        private readonly IDataInterface dataInterface;

        public LoginController(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            this.configuration = configuration;
            this.env = env;
            logger = loggerFactory.CreateLogger(typeof(LoginController));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string path = "dashboard")
        {
            await Task.CompletedTask;
            return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = $"{configuration.GetBasePath()}/{path}",
                IsPersistent = true
            });
        }
    }
}
