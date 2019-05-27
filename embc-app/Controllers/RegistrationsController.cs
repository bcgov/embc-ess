using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationService registrationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RegistrationsController(IRegistrationService registrationService, IHttpContextAccessor httpContextAccessor)
        {
            this.registrationService = registrationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id, [FromQuery] string reason)
        {
            var result = await registrationService.GetEvacueeRegistrationAsync(id);
            if (result != null && result.IsFinalized && string.IsNullOrWhiteSpace(reason))
            {
                return BadRequest($"must specify a reason for viewing a finalized event registration");
            }
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetOneSummary(string id)
        {
            var result = await registrationService.GetEvacueeRegistrationSummaryAsync(id);
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

            var result = await registrationService.CreateNewAsync(item);
            return Json(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest();
            }
            if (item != null && (!item.DeclarationAndConsent.HasValue || !item.DeclarationAndConsent.Value))
            {
                ModelState.AddModelError("DeclarationAndConsent", "Declaration And Consent must be set to 'True'");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.CompletedBy =
                new ViewModels.Volunteer
                {
                    Externaluseridentifier = httpContextAccessor?.HttpContext?.User?.FindFirstValue(EssClaimTypes.USER_ID)
                };

            await registrationService.UpdateAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var result = await registrationService.DeactivateEvacueeRegistrationAsync(id);
            return Ok();
        }
    }
}
