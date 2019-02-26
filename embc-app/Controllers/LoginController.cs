using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gov.Jag.Embc.Interfaces;

using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment _env;
        private readonly SiteMinderAuthOptions _options = new SiteMinderAuthOptions();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        
        const string BUSINESS_PROFILE_PAGE = "business-profile";

        public LoginController(IConfiguration configuration, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(LoginController));
            
        }

        [HttpGet]
        [Authorize]
        public ActionResult Login(string path)
        {
            // check to see if we have a local path.  (do not allow a redirect to another website)
            if (!string.IsNullOrEmpty(path) && (Url.IsLocalUrl(path) || (!_env.IsProduction() && path.Equals("headers"))))
            {
                // diagnostic feature for development - echo headers back.
                if ((!_env.IsProduction()) && path.Equals("headers"))
                {
                    StringBuilder html = new StringBuilder();
                    html.AppendLine("<html>");
                    html.AppendLine("<body>");
                    html.AppendLine("<b>Request Headers:</b>");
                    html.AppendLine("<ul style=\"list-style-type:none\">");
                    foreach (var item in Request.Headers)
                    {
                        html.AppendFormat("<li><b>{0}</b> = {1}</li>\r\n", item.Key, ExpandValue(item.Value));
                    }
                    html.AppendLine("</ul>");
                    html.AppendLine("</body>");
                    html.AppendLine("</html>");
                    ContentResult contentResult = new ContentResult();
                    contentResult.Content = html.ToString();
                    contentResult.ContentType = "text/html";
                    return contentResult;
                }
                return LocalRedirect(path);
            }
            else
            {
                string basePath = string.IsNullOrEmpty(Configuration["BASE_PATH"]) ? "/" : Configuration["BASE_PATH"];
                // we want to redirect to the dashboard if the user is a returning user.

                // get UserSettings from the session
                string temp = _httpContextAccessor.HttpContext.Session.GetString("UserSettings");
                string dashboard = "dashboard";

                if (string.IsNullOrEmpty(temp))
                {
                    dashboard = BUSINESS_PROFILE_PAGE;
                }
                else
                {
                    UserSettings userSettings = JsonConvert.DeserializeObject<UserSettings>(temp);
                    if (userSettings.IsNewUserRegistration || isBusinessProfileSubmitted(userSettings))
                    {
                        dashboard = BUSINESS_PROFILE_PAGE;
                    }
                }
                

                return Redirect(basePath + "/" + dashboard);
            }
        }

        private bool isBusinessProfileSubmitted(UserSettings userSettings)
        {
            var isSubmitted = false;
            // query the Dynamics system to get the account record.
            if (userSettings.AccountId != null && userSettings.AccountId.Length > 0)
            {
                var accountId = GuidUtility.SanitizeGuidString(userSettings.AccountId);

            }

            return isSubmitted;
        }

        /// <summary>
        /// Utility function used to expand headers.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string ExpandValue(IEnumerable<string> values)
        {
            StringBuilder value = new StringBuilder();

            foreach (string item in values)
            {
                if (value.Length > 0)
                {
                    value.Append(", ");
                }
                value.Append(item);
            }
            return value.ToString();
        }

        /// <summary>
        /// Injects an authentication token cookie into the response for use with the 
        /// SiteMinder authentication middleware
        /// </summary>
        [HttpGet]
        [Route("token/{userid}")]
        [AllowAnonymous]
        public virtual IActionResult GetDevAuthenticationCookie(string userId)
        {

            // if (_env.IsProduction()) return BadRequest("This API is not available outside a development environment.");

            if (string.IsNullOrEmpty(userId)) return BadRequest("Missing required userid query parameter.");

            if (userId.ToLower() == "default")
                userId = _options.DevDefaultUserId;

            // clear session
            HttpContext.Session.Clear();

			// expire "dev" user cookie
			string temp = HttpContext.Request.Cookies[_options.DevBCSCAuthenticationTokenKey];
            if (temp == null)
            {
                temp = "";
            }
            Response.Cookies.Append(
                _options.DevBCSCAuthenticationTokenKey,
                temp,
                new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1)
                }
            );
            // create new "dev" user cookie
            Response.Cookies.Append(
                _options.DevAuthenticationTokenKey,
                userId,
                new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            string basePath = string.IsNullOrEmpty(Configuration["BASE_PATH"]) ? "" : Configuration["BASE_PATH"];
            // always send the user to the business profile.
            string businessprofile = "business-profile";
            basePath += "/" + businessprofile;

            return Redirect(basePath);
        }

    }
}
