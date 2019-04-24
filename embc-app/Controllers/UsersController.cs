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
                appRoles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray(),
                name = principal.FindFirstValue(SiteMinderClaimTypes.Name),
                UserType = principal.FindFirstValue(SiteMinderClaimTypes.UserType),
                contactid = principal.FindFirstValue(ClaimTypes.Sid),
                id = principal.FindFirstValue(ClaimTypes.Upn),
                accountid = principal.FindFirstValue(SiteMinderClaimTypes.OrgId),
                ClientTimeoutWarningInMinutes = configuration.ClientTimeoutWarningInMinutes(),
                ClientTimeoutWarningDurationInMinutes = configuration.ClientTimeoutWarningDurationInMinutes()
            };

            return new JsonResult(user);
        }
    }
}
