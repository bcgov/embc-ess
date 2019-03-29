using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Authentication
{
    public static class SiteminderAuthentication2Extensions
    {
        public static AuthenticationBuilder AddSiteminderAuth2(this AuthenticationBuilder builder, Action<SiteMinderAuthOptions2> configureOptions)
        {
            return builder.AddScheme<SiteMinderAuthOptions2, SiteminderAuthenticationHandler2>(SiteMinderAuthOptions2.AuthenticationSchemeName, configureOptions);
        }
    }

    public class SiteMinderAuthOptions2 : AuthenticationSchemeOptions
    {
        public static string AuthenticationSchemeName => "site-minder-auth";
        public string Scheme => AuthenticationSchemeName;
    }

    public class SiteMinderToken
    {
        public string smgov_userguid;
        public string sm_universalid;
        public string sm_user;
        public string smgov_businessguid;
        public string smgov_businesslegalname;
        public string smgov_usertype;
        public string smgov_userdisplayname;

        public static SiteMinderToken CreateFromHttpHeaders(HttpRequest req)
        {
            return new SiteMinderToken
            {
                smgov_userguid = req.Headers["smgov_userguid "],
                sm_universalid = req.Headers["sm_universalid"],
                smgov_businessguid = req.Headers["smgov_businessguid"],
                sm_user = req.Headers["sm_user"],
                smgov_userdisplayname = req.Headers["smgov_userdisplayname"],
                smgov_businesslegalname = req.Headers["smgov_businesslegalname"],
                smgov_usertype = req.Headers["smgov_usertype"],
            };
        }

        public void AddHeadersToResponse(HttpResponse res)
        {
            res.Headers.Add("smgov_userguid", smgov_userguid);
            res.Headers.Add("smgov_userdisplayname", smgov_userdisplayname);
            res.Headers.Add("smgov_businesslegalname", smgov_businesslegalname);
            res.Headers.Add("sm_universalid", sm_universalid);
            res.Headers.Add("sm_user", sm_user);
            res.Headers.Add("smgov_businessguid", smgov_businessguid);
            res.Headers.Add("smgov_usertype", smgov_usertype);
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrEmpty(sm_universalid)
                && !string.IsNullOrEmpty(smgov_userdisplayname)
                && !string.IsNullOrEmpty(smgov_userguid)
                && !string.IsNullOrEmpty(smgov_usertype);
        }

        public override string ToString()
        {
            return $"smgov_userguid={smgov_userguid};" +
                $"sm_universalid={sm_universalid};" +
                $"smgov_userdisplayname={smgov_userdisplayname};" +
                $"smgov_businesslegalname={smgov_businesslegalname};" +
                $"smgov_usertype={smgov_usertype};" +
                $"smgov_businessguid={smgov_businessguid};" +
                $"sm_user={sm_user}";
        }
    }

    public class SiteminderAuthenticationHandler2 : AuthenticationHandler<SiteMinderAuthOptions2>
    {
        private readonly IDataInterface dataService;
        private readonly ILogger<SiteminderAuthenticationHandler2> logger;

        public SiteminderAuthenticationHandler2(IOptionsMonitor<SiteMinderAuthOptions2> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock, IDataInterface dataService)
            : base(options, loggerFactory, encoder, clock)
        {
            this.dataService = dataService;
            logger = loggerFactory.CreateLogger<SiteminderAuthenticationHandler2>();
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.HttpContext.User.Identity.IsAuthenticated) return AuthenticateResult.Success(new AuthenticationTicket(Request.HttpContext.User, Options.Scheme));
            if (Request.GetDisplayUrl().Contains(".js", System.StringComparison.OrdinalIgnoreCase)) return AuthenticateResult.NoResult();

            var userSettings = UserSettings.ReadUserSettings(Request.HttpContext);
            if (!userSettings.UserAuthenticated)
            {
                var token = SiteMinderToken.CreateFromHttpHeaders(Request);
                if (token == null || !token.IsValid())
                {
                    logger.LogError($"token is invalid: {token.ToString()}");
                    return AuthenticateResult.Fail("Not authenticated by SiteMinder");
                }
                var user = dataService.GetUserByExternalId(token.smgov_userguid);

                if (user == null)
                {
                    if (!await AccosiateVolunteerToUser(token.sm_universalid, token.smgov_userguid))
                    {
                        logger.LogError($"Volunteer not found for token: {token.ToString()}");
                        return AuthenticateResult.Fail($"No user with id '{token.sm_universalid}' was found");
                    }
                    user = dataService.GetUserByExternalId(token.smgov_userguid);
                }
                userSettings.AuthenticatedUser = user;
                userSettings.UserId = token.sm_universalid;
                userSettings.UserAuthenticated = true;
                userSettings.SiteMinderBusinessGuid = "";
                userSettings.SiteMinderGuid = token.smgov_userguid;
                userSettings.UserType = token.smgov_usertype;
                userSettings.AccountId = user.AccountId.ToString();
                userSettings.BusinessLegalName = token.smgov_businesslegalname;
                userSettings.SiteMinderBusinessGuid = token.smgov_businessguid;
                userSettings.IsNewUser = false;
                userSettings.ContactId = user.ContactId.ToString();
                userSettings.NewContact = null;
                userSettings.UserDisplayName = token.smgov_userdisplayname;
            }

            userSettings.Validate();
            UserSettings.SaveUserSettings(userSettings, Request.HttpContext);
            var result = AuthenticateResult.Success(new AuthenticationTicket(userSettings.AuthenticatedUser.ToClaimsPrincipal(Options.Scheme, userSettings.UserType), Options.Scheme));
            return result;
        }

        private async Task<bool> AccosiateVolunteerToUser(string bceidUserId, string bceIdGuid)
        {
            var volunteer = dataService.GetVolunteerByBceidUserId(bceidUserId);
            if (volunteer == null) return false;

            volunteer.Externaluseridentifier = bceIdGuid;
            await dataService.UpdatePersonAsync(volunteer);
            return true;
        }
    }
}
