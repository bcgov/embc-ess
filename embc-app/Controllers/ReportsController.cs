using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace embc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IMediator mediator;

        public ReportsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("registration/audit/{id}")]
        public async Task<IActionResult> GetRegistrationAudit(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return BadRequest($"'{id}' not a valid ESS file number");
            var registration = await mediator.Send(new RegistrationSummaryQueryRequest(id));
            if (registration == null) return NotFound();
            var results = await mediator.Send(new RegistrationAuditQueryRequest(essFileNumber));

            Response.Headers.Add("Content-Disposition", $"inline; filename=\"{id}.csv\"");
            return Content(results
                .Select(e => new
                {
                    e.EssFileNumber,
                    e.UserName,
                    Date = TimeZoneConverter.GetFormatedLocalDateTime(e.DateViewed),  //Tue, 11 Jun 2019 11:36:22 PDT
                    e.Reason
                })
                .ToCSV(), "text/csv");
        }

        [HttpGet("registration/audit/{id}/json")]
        public async Task<IActionResult> GetRegistrationAuditJson(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return BadRequest($"'{id}' not a valid ESS file number");
            var registration = await mediator.Send(new RegistrationSummaryQueryRequest(id));
            if (registration == null) return NotFound();
            var results = await mediator.Send(new RegistrationAuditQueryRequest(essFileNumber));

            return Json(results);
        }
    }
}
