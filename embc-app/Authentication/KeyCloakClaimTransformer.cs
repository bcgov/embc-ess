using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Authentication
{
    public class KeyCloakClaimTransformer : IClaimsTransformation
    {
        private static readonly IEnumerable<string> ProvincialAdminRoles = new[] { "role_everyone", "role_volunteer", "role_local_authority", "role_provincial_admin" };
        private static readonly IEnumerable<string> LocalAuthorityRoles = new[] { "role_everyone", "role_volunteer", "role_local_authority" };
        private static readonly IEnumerable<string> VolunteerRoles = new[] { "role_everyone", "role_volunteer" };
        private static readonly IEnumerable<string> EvacueeRoles = new[] { "role_everyone" };
        private readonly IDataInterface dataInterface;

        /// <summary>
        /// Transforms a principal returned from KeyCloak authentication service
        /// </summary>
        /// <param name="dataInterface"></param>
        public KeyCloakClaimTransformer(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var userGuid = principal.FindFirstValue(ClaimTypes.Sid);
            var userName = principal.FindFirstValue("preferred_username")?.Split('@')[0];
            var type = principal.FindFirstValue("identity_source");
            bool isAdmin = "admin".Equals(principal.FindFirstValue(ClaimTypes.Role), StringComparison.InvariantCultureIgnoreCase);

            var transformedClaims = new List<Claim>();
            transformedClaims.Add(new Claim(ClaimTypes.Sid, userGuid));
            transformedClaims.Add(new Claim(ClaimTypes.Upn, userName));
            transformedClaims.Add(new Claim(ClaimTypes.Name, principal.FindFirstValue("name")));
            transformedClaims.Add(new Claim(SiteMinderClaimTypes.NAME, principal.FindFirstValue("name")));
            if (type != null)
            {
                if (type.Equals("bceid", StringComparison.InvariantCultureIgnoreCase)) transformedClaims.Add(new Claim(SiteMinderClaimTypes.USER_TYPE, "business"));
                if (type.Equals("idir", StringComparison.InvariantCultureIgnoreCase)) transformedClaims.Add(new Claim(SiteMinderClaimTypes.USER_TYPE, "internal"));
            }

            if (isAdmin)
            {
                transformedClaims.AddRange(ProvincialAdminRoles.Select(r => new Claim(ClaimTypes.Role, r))); //provincial admin roles
                transformedClaims.Add(new Claim(EssClaimTypes.USER_ID, userGuid));
            }
            else
            {
                var user = await dataInterface.GetVolunteerByBceidUserNameAsync(userName);
                var roles = user == null
                    ? EvacueeRoles  //evacuee default role
                    : user.IsPrimaryContact ?? false ? LocalAuthorityRoles : VolunteerRoles; //volunteer/volunteer admin roles

                transformedClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                if (user != null)
                {
                    transformedClaims.Add(new Claim(EssClaimTypes.ORG_ID, user.Organization.Id));
                    transformedClaims.Add(new Claim(EssClaimTypes.USER_ID, user.Id));
                }
            }

            return new ClaimsPrincipal(new ClaimsIdentity(transformedClaims, principal.Identity.AuthenticationType));
        }
    }
}
