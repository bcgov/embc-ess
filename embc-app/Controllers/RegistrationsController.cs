using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Services.Registrations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationsController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMediator mediator;

        public RegistrationsController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id, [FromQuery] string reason)
        {
            var result = await mediator.Send(new RegistrationQueryRequest(id, reason));
            switch (result.Status)
            {
                case RegistrationQueryResponse.ResponseStatus.Success:
                    return Json(result.Registration);

                case RegistrationQueryResponse.ResponseStatus.NotFound:
                    return NotFound(id);

                case RegistrationQueryResponse.ResponseStatus.Error:
                    return BadRequest(result.FailureReason);

                default:
                    throw new InvalidOperationException($"invalid status {result.Status} returned for registration {id}");
            }
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetOneSummary(string id)
        {
            var result = await mediator.Send(new RegistrationSummaryQueryRequest(id));
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
            // if the included registration isn't null, and the consent isn't null AND is set true
            if (item != null && (!item.DeclarationAndConsent.HasValue || !item.DeclarationAndConsent.Value))
            {
                ModelState.AddModelError("DeclarationAndConsent", "Declaration And Consent must be set to 'True'");
            }
            // the model must be valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // set the completedBy
            item.CompletedBy = new ViewModels.Volunteer
            {
                // the external user identifier is set to the user ID included in the security claim
                Id = httpContextAccessor?.HttpContext?.User?.FindFirstValue(EssClaimTypes.USER_ID)
            };

            // return the complete registration so that the ESS number can be displayed to a self-registering user
            return Json(await mediator.Send(new CreateNewRegistrationCommand(item)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            // the id must be included in the route, the registration must be included, the id on the route must match the one in the payload
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id) return BadRequest(id);
            // if the declaration and consent flag is not set or is false the poster should fix this because they have not provided consent so we cannot accept their registration.
            if (!item.DeclarationAndConsent.HasValue || !item.DeclarationAndConsent.Value)
            {
                ModelState.AddModelError("DeclarationAndConsent", "Declaration And Consent must be set to 'True'");
            }

            // the model must be valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // set who completed the item
            item.CompletedBy = new ViewModels.Volunteer
            {
                // the external user identifier gets set to the user ID included in the claim
                Id = httpContextAccessor?.HttpContext?.User?.FindFirstValue(EssClaimTypes.USER_ID) ?? "System"
            };

            // update the registration
            await mediator.Send(new UpdateRegistrationCommand(item));

            // return 200
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await mediator.Send(new DeactivateRegistrationCommand(id));

            if (!result) return NotFound(id);
            return Ok();
        }
    }
}
