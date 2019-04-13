using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Public.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected ClaimsPrincipal CurrentUser => _httpContextAccessor.HttpContext.User;

        [HttpGet("current")]
        //[RequiresPermission(Permission.Login, Permission.NewUserRegistration)]

        public virtual IActionResult UsersCurrentGet()
        {
            SiteMinderAuthOptions siteMinderAuthOptions = new SiteMinderAuthOptions();

            // determine if we are a new registrant.
            string temp = _httpContextAccessor.HttpContext.Session.GetString("UserSettings");
            UserSettings userSettings = JsonConvert.DeserializeObject<UserSettings>(temp);
            ViewModels.User user = new ViewModels.User()
            {
                id = userSettings.UserId,
                contactid = userSettings.ContactId,
                accountid = userSettings.AccountId,
                businessname = userSettings.BusinessLegalName,
                name = userSettings.UserDisplayName,
                UserType = userSettings.UserType,
                appRoles = userSettings.AppRoles
            };

            if (userSettings.IsNewUser)
            {
                user.isNewUser = true;
                // get details from the headers.

                user.lastname = CommonDynamicsExtensions.GetLastName(user.name);
                user.firstname = CommonDynamicsExtensions.GetFirstName(user.name);
                user.accountid = userSettings.AccountId;

                string siteminderBusinessGuid = _httpContextAccessor.HttpContext.Request.Headers[siteMinderAuthOptions.SiteMinderBusinessGuidKey];
                string siteminderUserGuid = _httpContextAccessor.HttpContext.Request.Headers[siteMinderAuthOptions.SiteMinderUserGuidKey];

                user.contactid = string.IsNullOrEmpty(siteminderUserGuid) ? userSettings.ContactId : siteminderUserGuid;
                user.accountid = string.IsNullOrEmpty(siteminderBusinessGuid) ? userSettings.AccountId : siteminderBusinessGuid;
            }
            else
            {
                user.lastname = userSettings.AuthenticatedUser.Surname;
                user.firstname = userSettings.AuthenticatedUser.GivenName;
                user.email = userSettings.AuthenticatedUser.Email;
                user.isNewUser = false;
            }

            return new JsonResult(user);
        }
    }
}
