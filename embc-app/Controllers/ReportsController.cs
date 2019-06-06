using Gov.Jag.Embc.Public.Services.Registrations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace embc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("registration/audit/{id}")]
        public async Task<IActionResult> GetRegistrationAudit(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return BadRequest($"'{id}' not a valid ess file number");
            var registration = await mediator.Send(new RegistrationSummaryQueryRequest(id));
            if (registration == null) return NotFound();
            var auditTrail = await mediator.Send(new RegistrationAuditQueryRequest(essFileNumber));

            return Ok(auditTrail);
        }
    }
}
