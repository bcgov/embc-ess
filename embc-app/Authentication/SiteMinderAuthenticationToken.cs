using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.Authentication
{
    public static class SiteMinderClaimTypes
    {
        public static string UserType = "sm.usertype";
        public static string Name = "sm.name";
        public static string OrgId = "sm.orgid";
    }

    public class SiteMinderAuthenticationToken
    {
        public string smgov_userguid;
        public string sm_universalid;
        public string sm_user;
        public string smgov_businessguid;
        public string smgov_businesslegalname;
        public string smgov_usertype;
        public string smgov_userdisplayname;

        private readonly static SiteMinderAuthenticationToken Anonymous = new SiteMinderAuthenticationToken();
        public const string COOKIE_NAME = "sm.token";

        public static SiteMinderAuthenticationToken GetFromHttpHeaders(HttpRequest req)
        {
            return new SiteMinderAuthenticationToken
            {
                smgov_userguid = req.Headers["smgov_userguid"],
                sm_universalid = req.Headers["sm_universalid"],
                smgov_businessguid = req.Headers["smgov_businessguid"],
                sm_user = req.Headers["sm_user"],
                smgov_userdisplayname = req.Headers["smgov_userdisplayname"],
                smgov_businesslegalname = req.Headers["smgov_businesslegalname"],
                smgov_usertype = req.Headers["smgov_usertype"],
            };
        }

        public static SiteMinderAuthenticationToken GetFromCookie(HttpRequest req)
        {
            var str = req.Cookies[COOKIE_NAME]?.Base64Decode();
            if (string.IsNullOrWhiteSpace(str)) return Anonymous;

            var dict = str.Split(';').Select(p => p.Split('=')).ToDictionary(v => v[0], v => v[1]);
            return new SiteMinderAuthenticationToken
            {
                smgov_userguid = dict["smgov_userguid"],
                sm_universalid = dict["sm_universalid"],
                smgov_businessguid = dict["smgov_businessguid"],
                sm_user = dict["sm_user"],
                smgov_userdisplayname = dict["smgov_userdisplayname"],
                smgov_businesslegalname = dict["smgov_businesslegalname"],
                smgov_usertype = dict["smgov_usertype"]
            };
        }

        public static void AddToResponse(SiteMinderAuthenticationToken token, HttpResponse res)
        {
            res.Cookies.Append(COOKIE_NAME, token.ToString().Base64Encode(), new CookieOptions()
            {
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.Now.AddSeconds(30),
                HttpOnly = true
            });
        }

        public bool IsAnonymous()
        {
            return
              string.IsNullOrEmpty(sm_universalid)
              || string.IsNullOrEmpty(smgov_userdisplayname)
              || string.IsNullOrEmpty(smgov_userguid)
              || string.IsNullOrEmpty(smgov_usertype);
        }

        public bool IsInternal()
        {
            return "internal".Equals(smgov_usertype, StringComparison.OrdinalIgnoreCase);
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

        public IEnumerable<Claim> ToClaims()
        {
            return new[]
            {
                new Claim(SiteMinderClaimTypes.UserType, smgov_usertype),
                new Claim(ClaimTypes.Sid, smgov_userguid),
                new Claim(SiteMinderClaimTypes.Name, smgov_userdisplayname),
                new Claim(ClaimTypes.Upn, sm_universalid),
                new Claim(SiteMinderClaimTypes.OrgId, smgov_businessguid)
            };
        }
    }
}
