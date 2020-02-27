using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services
{
    public class CurrentUserService : ICurrentUser
    {
        private string _displayName;
        
        public User CurrentUser { get; }

        public CurrentUserService(IHttpContextAccessor context)
        {
            ClaimsPrincipal principal = context.HttpContext.User;
            CurrentUser = new User()
            {
                appRoles  = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray(),
                name      = principal.FindFirstValue(SiteMinderClaimTypes.NAME), //Keycloak displayName field
                firstname = principal.FindFirstValue(ClaimTypes.GivenName),
                lastname  = principal.FindFirstValue(ClaimTypes.Surname),
                UserType  = principal.FindFirstValue(SiteMinderClaimTypes.USER_TYPE),
                contactid = principal.FindFirstValue(EssClaimTypes.USER_ID),
                id        = principal.FindFirstValue(ClaimTypes.Upn),
                accountid = principal.FindFirstValue(EssClaimTypes.ORG_ID)
            };
        }

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                // Because of inconsistent formats of names, we'll try separating names on spaces, then commas.
                string[] spaceSplit = CurrentUser.name.Split(' ');
                string[] commaSplit = CurrentUser.name.Split(',');

                if (spaceSplit.Length >= 2)
                {
                    GenerateDisplayName(spaceSplit);
                }
                else if (commaSplit.Length >= 2)
                {
                    GenerateDisplayName(commaSplit);
                }
                else
                {
                    // Can't determine how to split the name - use it as is
                    _displayName = CurrentUser.name;
                }
            }
            return _displayName;
        }


        private void GenerateDisplayName(string[] nameArray)
        {
            string fName = nameArray[0];
            string lName = nameArray[1];
            if (lName.Length >= 1)
            {
                lName = lName.Substring(0, 1);
            }
            _displayName = $"{fName} {lName}";
            // clean up any rogue commas
            _displayName = _displayName.Replace(",", string.Empty).Trim();

        }
    }
}
