using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Authentication
{
    public class SiteMinderAuthOptions : AuthenticationSchemeOptions
    {
        public const string AuthenticationSchemeName = "site-minder-auth";
        public string Scheme => AuthenticationSchemeName;
    }

    public static class SiteminderAuthenticationExtensions
    {
        /// <summary>
        /// Add Authentication Handler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSiteminderAuth(this AuthenticationBuilder builder, Action<SiteMinderAuthOptions> configureOptions)
        {
            return builder.AddScheme<SiteMinderAuthOptions, SiteminderAuthenticationHandler>(SiteMinderAuthOptions.AuthenticationSchemeName, configureOptions);
        }
    }

    public class SiteminderAuthenticationHandler : AuthenticationHandler<SiteMinderAuthOptions>
    {
        private readonly ILogger logger;
        private readonly IDataInterface dataInterface;
        private readonly IHostingEnvironment environment;

        public SiteminderAuthenticationHandler(IOptionsMonitor<SiteMinderAuthOptions> configureOptions,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IDataInterface dataInterface,
            IHostingEnvironment environment)
             : base(configureOptions, loggerFactory, encoder, clock)
        {
            this.environment = environment;
            this.dataInterface = dataInterface;
            logger = loggerFactory.CreateLogger(typeof(SiteminderAuthenticationHandler));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var smAuthToken = SiteMinderAuthenticationToken.GetFromHttpHeaders(Request);
            if (!environment.IsProduction() && smAuthToken.IsAnonymous())
            {
                smAuthToken = SiteMinderAuthenticationToken.GetFromCookie(Request);
                Response.Cookies.Delete(SiteMinderAuthenticationToken.COOKIE_NAME);
            }

            logger.LogDebug($"smAuthToken: {smAuthToken.ToString()}");
            var claims = Context.Session.GetString("app.principal");
            if (!string.IsNullOrEmpty(claims))
            {
                var principal = claims.FromJwt();
                logger.LogDebug($"Success (session): {principal.Identity.Name}");
                return AuthenticateResult.Success(new AuthenticationTicket(principal, Options.Scheme));
            }
            if (smAuthToken.IsAnonymous())
            {
                logger.LogDebug($"NoResult");
                return AuthenticateResult.NoResult();
            }

            try
            {
                var principal = await CreatePrincipalFor(smAuthToken);
                Context.Session.SetString("app.principal", principal.ToJwt());
                logger.LogDebug($"Success (new): {principal.Identity.Name}");
                return AuthenticateResult.Success(new AuthenticationTicket(principal, Options.Scheme));
            }
            catch (ApplicationException e)
            {
                logger.LogError($"Fail: {e.Message}");
                return AuthenticateResult.Fail(e.Message);
            }
        }

        private async Task<ClaimsPrincipal> CreatePrincipalFor(SiteMinderAuthenticationToken smAuthToken)
        {
            var roles = new List<string>();
            roles.Add("role_everyone");

            if (smAuthToken.IsInternal())
            {
                //EMBC admin
                roles.Add("role_volunteer");
                roles.Add("role_local_authority");
                roles.Add("role_provincial_admin");
            }
            else
            {
                var volunteer = dataInterface.GetVolunteerByBceidUserId(smAuthToken.sm_universalid);
                if (volunteer == null) throw new ApplicationException("Volunteer not found");
                //Volunteer
                if (string.IsNullOrEmpty(volunteer.Externaluseridentifier))
                {
                    volunteer.Externaluseridentifier = smAuthToken.smgov_userguid;
                    await dataInterface.UpdateVolunteerAsync(volunteer);
                }
                if (volunteer.Externaluseridentifier != smAuthToken.smgov_userguid) throw new ApplicationException("Volunteer BCeID GUID does not match");
                if (volunteer.Organization.BCeIDBusinessGuid != smAuthToken.smgov_businessguid) throw new ApplicationException("Volunteer doesn't belong to the correct organization");

                roles.Add("role_volunteer");
                if (volunteer.IsAdministrator ?? false) roles.Add("role_local_authority");
            }
            var claims = new List<Claim>();
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            claims.AddRange(smAuthToken.ToClaims());
            return new ClaimsPrincipal(new ClaimsIdentity(claims, Options.Scheme));
        }
    }

    public static class JwtEx
    {
        public static string ToJwt(this ClaimsPrincipal principal)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.OutboundClaimTypeMap.Clear();
            var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = (ClaimsIdentity)principal.Identity,
                Audience = "self",
                Issuer = "self",
            });

            return token;
        }

        public static ClaimsPrincipal FromJwt(this string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            var claims = handler.ReadJwtToken(jwt).Claims;
            return new ClaimsPrincipal(new ClaimsIdentity(claims, SiteMinderAuthOptions.AuthenticationSchemeName));
        }
    }
}
