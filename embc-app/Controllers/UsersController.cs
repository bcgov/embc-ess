using Gov.Jag.Embc.Public.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor ctx;

        public UsersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.ctx = httpContextAccessor;
        }

        protected ClaimsPrincipal CurrentUser => ctx.HttpContext.User;

        [HttpGet("current")]
        public virtual IActionResult UsersCurrentGet()
        {
            var principal = HttpContext.User;
            ViewModels.User user = new ViewModels.User()
            {
                id = userSettings.UserId,
                contactid = userSettings.ContactId,
                accountid = userSettings.AccountId,
                businessname = userSettings.BusinessLegalName,
                name = userSettings.UserDisplayName,
                UserType = userSettings.UserType,
                appRoles = userSettings.AppRoles,
                ClientTimeoutWarningInMinutes = configuration.UserTimeoutWarningInMinutes(),
                ClientTimeoutWarningDurationInMinutes = configuration.UserTimeoutWarningDurationInMinutes()
            };

            return new JsonResult(user);
        }
    }
}
