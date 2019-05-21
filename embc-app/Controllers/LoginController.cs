using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("login")]
    [Authorize]
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private readonly IHttpContextAccessor httpCtx;
        private readonly ILogger logger;
        private readonly IDataInterface dataInterface;

        public LoginController(IConfiguration configuration, IHostingEnvironment env, IHttpContextAccessor httpCtx, ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            this.configuration = configuration;
            this.env = env;
            this.httpCtx = httpCtx;
            logger = loggerFactory.CreateLogger(typeof(LoginController));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string path = "dashboard")
        {
            if (!env.IsProduction() && "headers".Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                return Content(string.Join(Environment.NewLine, Request.Headers.Select(header => $"{header.Key}={string.Join(",", header.Value.ToArray())}")), "text/plain", Encoding.UTF8);
            }

            if (ControllerContext.HttpContext.User == null || !ControllerContext.HttpContext.User.Identity.IsAuthenticated) return Unauthorized();

            return await Task.FromResult(LocalRedirect($"{configuration["BASE_PATH"]}/{path}"));
        }

        [HttpGet]
        [Route("token/{userName}")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginDevelopment(string userName)
        {
            if (env.IsProduction() || string.IsNullOrWhiteSpace(userName)) return Unauthorized();

            HttpContext.Session.Clear();

            var secToken = await (userName.StartsWith("idir", StringComparison.OrdinalIgnoreCase)
                ? CreateDevTokenForIdir(userName)
                : CreateDevTokenForBceid(userName)
            );

            SiteMinderAuthenticationToken.AddToResponse(secToken, Response);

            return LocalRedirect($"{configuration["BASE_PATH"]}/login");
        }

        private async Task<SiteMinderAuthenticationToken> CreateDevTokenForIdir(string userName)
        {
            var guidBytes = Guid.Empty.ToByteArray();
            guidBytes[guidBytes.Length - 1] += (byte)userName.GetHashCode();
            return await Task.FromResult(new SiteMinderAuthenticationToken
            {
                smgov_businessguid = null,
                smgov_businesslegalname = null,
                smgov_userdisplayname = $"{userName} (IDIR)",
                smgov_userguid = new Guid(guidBytes).ToString(),
                smgov_usertype = "internal",
                sm_universalid = userName,
                sm_user = userName
            });
        }

        private async Task<SiteMinderAuthenticationToken> CreateDevTokenForBceid(string userName)
        {
            //TODO: convert GetVolunteerByBceidUserId to async
            var volunteer = await Task.FromResult(dataInterface.GetVolunteerByBceidUserId(userName));
            if (volunteer == null) return null;
            var secToken = new SiteMinderAuthenticationToken
            {
                smgov_businessguid = volunteer.Organization.BCeIDBusinessGuid,
                smgov_businesslegalname = volunteer.Organization.LegalName,
                smgov_userdisplayname = $"{volunteer.LastName}, {volunteer.FirstName}",
                smgov_userguid = volunteer.Externaluseridentifier ?? Guid.NewGuid().ToString(),
                smgov_usertype = "business",
                sm_universalid = volunteer.BceidAccountNumber,
                sm_user = volunteer.BceidAccountNumber
            };
            return secToken;
        }
    }
}
