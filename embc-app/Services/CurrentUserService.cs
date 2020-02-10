using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services
{
    public class CurrentUserService
    {
        private IHttpContextAccessor _context;
        private ClaimsPrincipal _principal;
        private User _user;
        public CurrentUserService(IHttpContextAccessor context)
        {
            _context   = context;
            _principal = _context.HttpContext.User;
        }

        public User GetCurrentUser()
        {
            if (_user == null)
            {
                _user = new User()
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

            return _user;
        }
    }
}
