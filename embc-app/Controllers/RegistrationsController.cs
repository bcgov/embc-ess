using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface dataInterface;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;
        private readonly IUrlHelper urlHelper;
        private readonly IEmailSender emailSender;

        public RegistrationsController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IDataInterface dataInterface,
            IEmailSender emailSender,
            IUrlHelper urlHelper
        )
        {
            this.emailSender = emailSender;
            Configuration = configuration;
            this.dataInterface = dataInterface;
            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(RegistrationsController));
            this.env = env;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = nameof(GetAll))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters queryParameters)
        {
            try
            {
                var results = await dataInterface.GetRegistrationsAsync(queryParameters);

                var paginationMetadata = new PaginationMetadata()
                {
                    CurrentPage = results.GetCurrentPage(),
                    PageSize = results.Limit,
                    TotalCount = results.TotalItemCount,
                    TotalPages = results.GetTotalPages()
                };

                return Json(new
                {
                    data = results,
                    metadata = paginationMetadata
                });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await dataInterface.GetRegistrationAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ViewModels.Registration item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (item != null && item.Id != null) item.Id = null;
                var result = await dataInterface.CreateRegistrationAsync(item);
                if (!string.IsNullOrWhiteSpace(result.HeadOfHousehold.Email))
                {
                    var registrationEmail = CreateEmailMessageForRegistration(result);
                    emailSender.Send(registrationEmail);
                }
                return Json(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        private EmailMessage CreateEmailMessageForRegistration(Registration registration)
        {
            var subject = "Registration completed successfully";
            var body = "<h2>Evacuee Registration Success</h2><br/>" + "<b>What you need to know:</b><br/><br/>" +
               $"Your ESS File Number is: <b>{registration.EssFileNumber}</b>";

            if (registration.IncidentTask == null)
            {
                body += "<br/><br/>" +
                   "- If you do not require support services, no further action is needed.<br/> " +
                   "- If services are required, please report to your nearest Reception Centre." +
                   " An updated list of reception centres can be found at <a href='https://www.emergencyinfobc.gov.bc.ca/'>EmergencyInfoBC</a>.<br/>" +
                   "- If you are at a Reception Centre, proceed to one of the ESS team members on site who will be able to assist you with completing your registration.<br/>" +
                   "- Don’t forget to bring your evacuee registration number with you to the Reception Centre.";
            }

            return new EmailMessage(registration.HeadOfHousehold.Email, subject, body);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            if (id != null && item.Id != null && id != item.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await dataInterface.UpdateRegistrationAsync(item);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
