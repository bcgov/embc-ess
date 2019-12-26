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

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var transformedClaims = new List<Claim>();
            switch (principal.FindFirstValue(ClaimTypes.Role)?.ToLowerInvariant())
            {
                case "admin":
                    transformedClaims.AddRange(ProvincialAdminRoles.Select(r => new Claim(ClaimTypes.Role, r)));
                    break;

                case "local-authority":
                    transformedClaims.AddRange(LocalAuthorityRoles.Select(r => new Claim(ClaimTypes.Role, r)));
                    break;

                case "volunteer":
                    transformedClaims.AddRange(VolunteerRoles.Select(r => new Claim(ClaimTypes.Role, r)));
                    break;

                default:
                    transformedClaims.AddRange(EvacueeRoles.Select(r => new Claim(ClaimTypes.Role, r)));
                    break;
            }
            transformedClaims.Add(principal.FindFirst(ClaimTypes.Sid));
            transformedClaims.Add(new Claim(ClaimTypes.Upn, principal.FindFirstValue("preferred_username")?.Split('@')[0]));
            transformedClaims.Add(new Claim(ClaimTypes.Name, principal.FindFirstValue("name")));
            transformedClaims.Add(new Claim(SiteMinderClaimTypes.NAME, principal.FindFirstValue("name")));
            var type = principal.FindFirstValue("identity_source");
            if (type != null)
            {
                if (type.Equals("bceid", StringComparison.InvariantCultureIgnoreCase)) transformedClaims.Add(new Claim(SiteMinderClaimTypes.USER_TYPE, "business"));
                if (type.Equals("idir", StringComparison.InvariantCultureIgnoreCase)) transformedClaims.Add(new Claim(SiteMinderClaimTypes.USER_TYPE, "internal"));
            }
            transformedClaims.Add(new Claim(EssClaimTypes.ORG_ID, ""));
            transformedClaims.Add(new Claim(EssClaimTypes.USER_ID, ""));

            return await Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(transformedClaims, principal.Identity.AuthenticationType)));
        }
    }
}
