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

            //TODO: refactor client and server property names to match claim names in order to simplify the code readability
            ViewModels.User user = new ViewModels.User()
            {
                appRoles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray(),
                name = principal.FindFirstValue(ClaimTypes.Name),
                UserType = principal.FindFirstValue(EssClaimTypes.USER_TYPE),
                contactid = principal.FindFirstValue(EssClaimTypes.USER_ID),
                id = principal.FindFirstValue(ClaimTypes.Upn),
                accountid = principal.FindFirstValue(EssClaimTypes.ORG_ID),
                ClientTimeoutWarningInMinutes = configuration.UserTimeoutWarningInMinutes(),
                ClientTimeoutWarningDurationInMinutes = configuration.UserTimeoutWarningInMinutes()
            };

            return new JsonResult(user);
        }
    }
}
