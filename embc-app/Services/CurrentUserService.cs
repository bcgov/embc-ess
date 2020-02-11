using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.Services
{
    public class CurrentUserService : ICurrentUser
    {
        private readonly ClaimsPrincipal _principal;
        public User CurrentUser { get; }

        public CurrentUserService(IHttpContextAccessor context)
        {
            _principal = context.HttpContext.User;
            CurrentUser = new User()
            {
                appRoles  = _principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray(),
                name      = _principal.FindFirstValue(SiteMinderClaimTypes.NAME),
                firstname = _principal.FindFirstValue(ClaimTypes.GivenName),
                lastname  = _principal.FindFirstValue(ClaimTypes.Surname),
                UserType  = _principal.FindFirstValue(SiteMinderClaimTypes.USER_TYPE),
                contactid = _principal.FindFirstValue(EssClaimTypes.USER_ID),
                id        = _principal.FindFirstValue(ClaimTypes.Upn),
                accountid = _principal.FindFirstValue(EssClaimTypes.ORG_ID)
            };
        }

    }
}
