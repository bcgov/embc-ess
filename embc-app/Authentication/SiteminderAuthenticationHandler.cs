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
        private readonly ILogger _logger;
        private readonly IHostingEnvironment env;

        /// <summary>
        /// Siteminder Authentication Constructir
        /// </summary>
        /// <param name="configureOptions"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public SiteminderAuthenticationHandler(IOptionsMonitor<SiteMinderAuthOptions> configureOptions, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock, IHostingEnvironment env)
            : base(configureOptions, loggerFactory, encoder, clock)
        {
            this.env = env;
            _logger = loggerFactory.CreateLogger(typeof(SiteminderAuthenticationHandler));
        }

        /// <summary>
        /// Process Authentication Request
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
#if DEBUG
            if (env.IsDevelopment() && Request.Path.StartsWithSegments(new PathString("/api")) && !Request.Path.StartsWithSegments(new PathString("/api/users")))
            {
                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(Options.Scheme)), Options.Scheme));
            }
#endif
            ClaimsPrincipal principal = new ClaimsPrincipal();

            // get siteminder headers
            _logger.LogError("Parsing the HTTP headers for SiteMinder authentication credential");

            SiteMinderAuthOptions options = new SiteMinderAuthOptions();
            bool isDeveloperLogin = false;
            bool isBCSCDeveloperLogin = false;
            try
            {
                HttpContext context = Request.HttpContext;

                IDataInterface _dataInterface = (IDataInterface)context.RequestServices.GetService(typeof(IDataInterface));

                IHostingEnvironment hostingEnv = (IHostingEnvironment)context.RequestServices.GetService(typeof(IHostingEnvironment));

                UserSettings userSettings = new UserSettings();

                string userId = null;
                string devCompanyId = null;
                string siteMinderGuid = "";
                string siteMinderBusinessGuid = "";
                string siteMinderUserType = "";

                // **************************************************
                // If this is an Error or Authentiation API - Ignore
                // **************************************************
                string url = context.Request.GetDisplayUrl().ToLower();

                if (url.Contains(".js"))
                {
                    return AuthenticateResult.NoResult();
                }

                // **************************************************
                // Check if we have a Dev Environment Cookie
                // **************************************************
                //if (!hostingEnv.IsProduction())
                //{
                // check for a fake BCeID login in dev mode
                string temp = context.Request.Cookies[options.DevAuthenticationTokenKey];

                if (string.IsNullOrEmpty(temp)) // could be an automated test user.
                {
                    temp = context.Request.Headers["DEV-USER"];
                }

                if (!string.IsNullOrEmpty(temp))
                {
                    if (temp.Contains("::"))
                    {
                        var temp2 = temp.Split("::");
                        userId = temp2[0];
                        if (temp2.Length >= 2)
                            devCompanyId = temp2[1];
                        else
                            devCompanyId = temp2[0];
                    }
                    else
                    {
                        userId = temp;
                        devCompanyId = temp;
                    }
                    isDeveloperLogin = true;

                    _logger.LogError("Got user from dev cookie = " + userId + ", company = " + devCompanyId);
                }
                else
                {
                    // same set of tests for a BC Services Card dev login
                    temp = context.Request.Cookies[options.DevBCSCAuthenticationTokenKey];

                    if (string.IsNullOrEmpty(temp)) // could be an automated test user.
                    {
                        temp = context.Request.Headers["DEV-BCSC-USER"];
                    }

                    if (!string.IsNullOrEmpty(temp))
                    {
                        userId = temp;
                        isBCSCDeveloperLogin = true;

                        _logger.LogError("Got user from dev cookie = " + userId);
                    }
                }
                //}

                // **************************************************
                // Check if the user session is already created
                // **************************************************
                try
                {
                    _logger.LogInformation("Checking user session");
                    userSettings = UserSettings.ReadUserSettings(context);
                    _logger.LogError("UserSettings found: " + userSettings.GetJson());
                }
                catch
                {
                    //do nothing
                    _logger.LogError("No UserSettings found");
                }

                // is user authenticated - if so we're done
                if ((userSettings.UserAuthenticated && string.IsNullOrEmpty(userId)) ||
                    (userSettings.UserAuthenticated && !string.IsNullOrEmpty(userId) &&
                     !string.IsNullOrEmpty(userSettings.UserId) && userSettings.UserId == userId))
                {
                    _logger.LogError("User already authenticated with active session: " + userSettings.UserId);
                    principal = userSettings.AuthenticatedUser.ToClaimsPrincipal(options.Scheme, userSettings.UserType);
                    return AuthenticateResult.Success(new AuthenticationTicket(principal, null, Options.Scheme));
                }

                string smgov_userdisplayname = context.Request.Headers["smgov_userdisplayname"];
                if (!string.IsNullOrEmpty(smgov_userdisplayname))
                {
                    userSettings.UserDisplayName = smgov_userdisplayname;
                }

                string smgov_businesslegalname = context.Request.Headers["smgov_businesslegalname"];
                if (!string.IsNullOrEmpty(smgov_businesslegalname))
                {
                    userSettings.BusinessLegalName = smgov_businesslegalname;
                }

                // **************************************************
                // Authenticate based on SiteMinder Headers
                // **************************************************
                _logger.LogError("Parsing the HTTP headers for SiteMinder authentication credential");

                // At this point userID would only be set if we are logging in through as a DEV user

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogError("Getting user data from headers");

                    userId = context.Request.Headers[options.SiteMinderUserNameKey];
                    if (string.IsNullOrEmpty(userId))
                    {
                        userId = context.Request.Headers[options.SiteMinderUniversalIdKey];
                    }

                    siteMinderGuid = context.Request.Headers[options.SiteMinderUserGuidKey];
                    siteMinderBusinessGuid = context.Request.Headers[options.SiteMinderBusinessGuidKey];
                    siteMinderUserType = context.Request.Headers[options.SiteMinderUserTypeKey];

                    // **************************************************
                    // Validate credentials
                    // **************************************************
                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogError(options.MissingSiteMinderUserIdError);
                        return AuthenticateResult.Fail(options.MissingSiteMinderGuidError);
                    }

                    if (string.IsNullOrEmpty(siteMinderGuid))
                    {
                        _logger.LogError(options.MissingSiteMinderGuidError);
                        return AuthenticateResult.Fail(options.MissingSiteMinderGuidError);
                    }
                    if (string.IsNullOrEmpty(siteMinderUserType))
                    {
                        _logger.LogError(options.MissingSiteMinderUserTypeError);
                        return AuthenticateResult.Fail(options.MissingSiteMinderUserTypeError);
                    }
                }
                else // DEV user, setup a fake session and SiteMinder headers.
                {
                    if (isDeveloperLogin)
                    {
                        _logger.LogError("Generating a Development user");
                        userSettings.BusinessLegalName = devCompanyId + " BusinessProfileName";
                        userSettings.UserDisplayName = userId + " BCeIDContactType";

                        // search for a matching user.
                        var existingContact = _dataInterface.GetVolunteerByName(userId, "BCeIDContactType");

                        if (existingContact != null)
                        {
                            siteMinderGuid = existingContact.Externaluseridentifier;
                        }
                        else
                        {
                            siteMinderGuid = GuidUtility.CreateIdForDynamics("contact", userSettings.UserDisplayName).ToString();
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
