using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Utils;
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
        private readonly ICurrentUser cus;

        public UsersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ICurrentUser userServ)
        {
            this.configuration = configuration;
            this.ctx = httpContextAccessor;
            this.cus = userServ;
        }

        protected ClaimsPrincipal CurrentUser => ctx.HttpContext.User;

        [HttpGet("current")]
        public virtual IActionResult UsersCurrentGet()
        {
            return new JsonResult(this.cus.CurrentUser);
        }
    }
}
