using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.Authentication
{
    public static class SiteMinderClaimTypes
    {
        public static string USER_TYPE = "sm.user_type";
        public static string NAME = "sm.name";
        public static string BUSINESS_GUID = "sm.business_guid";
    }

    public static class EssClaimTypes
    {
        public static string USER_ID = "ess.user_id";
        public static string ORG_ID = "ess.org_id";
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
        public const string SM_TOKEN_NAME = "sm.token";

        public static SiteMinderAuthenticationToken CreateFromFwdHeaders(HttpRequest req)
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

        public static SiteMinderAuthenticationToken CreateForDev(HttpRequest req)
        {
            var str = req.Cookies[SM_TOKEN_NAME]?.Base64Decode();
            if (string.IsNullOrEmpty(str)) str = req.Headers[SM_TOKEN_NAME].ToString().Base64Decode();
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
            res.Cookies.Append(SM_TOKEN_NAME, token.ToString().Base64Encode(), new CookieOptions()
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

        public bool IsExternal()
        {
            return "business".Equals(smgov_usertype, StringComparison.OrdinalIgnoreCase);
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
}
