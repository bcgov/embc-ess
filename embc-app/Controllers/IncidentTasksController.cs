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
    [Authorize]
    public class IncidentTasksController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface dataInterface;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;

        public IncidentTasksController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IDataInterface dataInterface
        )
        {
            Configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(IncidentTasksController));
            this.env = env;
            this.dataInterface = dataInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchQueryParameters searchQuery)
        {
            var items = await dataInterface.GetIncidentTasksAsync(searchQuery);

            return Json(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await dataInterface.GetIncidentTaskAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncidentTask item)
        {
            if (!item.StartDate.HasValue)
            {
                ModelState.AddModelError("StartDate", "Incident task must have a start date");
            }
            if (item.StartDate.HasValue && item.StartDate.Value > DateTime.Now)
            {
                ModelState.AddModelError("StartDate", "Incident start date cannot be in the future");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            item.Id = null;
            item.Active = true;
            var taskId = await dataInterface.CreateIncidentTaskAsync(item);
            return Json(await dataInterface.GetIncidentTaskAsync(taskId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] IncidentTask item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest();
            }
            if (!item.StartDate.HasValue)
            {
                ModelState.AddModelError("StartDate", "Incident task must have a start date");
            }
            if (item.StartDate.HasValue && item.StartDate.Value > DateTime.Now)
            {
                ModelState.AddModelError("StartDate", "Incident start date cannot be in the future");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataInterface.UpdateIncidentTaskAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var result = await dataInterface.DeactivateIncidentTaskAsync(id);
            return Ok();
        }
    }
}
