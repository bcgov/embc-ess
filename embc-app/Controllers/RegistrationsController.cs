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
                    return Json(result);

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
            if (item != null && (!item.DeclarationAndConsent.HasValue || !item.DeclarationAndConsent.Value))
            {
                ModelState.AddModelError("DeclarationAndConsent", "Declaration And Consent must be set to 'True'");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Json(await mediator.Send(new CreateNewRegistrationCommand(item)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id) return BadRequest(id);
            if (!item.DeclarationAndConsent.HasValue || !item.DeclarationAndConsent.Value)
            {
                ModelState.AddModelError("DeclarationAndConsent", "Declaration And Consent must be set to 'True'");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.CompletedBy = new ViewModels.Volunteer
            {
                Externaluseridentifier = httpContextAccessor?.HttpContext?.User?.FindFirstValue(EssClaimTypes.USER_ID)
            };

            await mediator.Send(new FinalizeRegistrationCommand(item));
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
