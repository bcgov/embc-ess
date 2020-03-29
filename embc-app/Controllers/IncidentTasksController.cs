using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        [HttpGet("getopenincidenttasks")]
        public async Task<IActionResult> GetOpenIncidentTasks([FromQuery] SearchQueryParameters searchQuery)
        {
            int limit  = searchQuery.Limit;
            int offset = searchQuery.Offset;
            var items  = await dataInterface.GetOpenIncidentTasksAsync(limit, offset);
            
            return Json(items);
        }

        [HttpGet("getOpenAndClosedIncidentTaskMetadata")]
        public async Task<IActionResult> GetOpenAndClosedIncidentTaskMetadata([FromQuery] SearchQueryParameters searchQuery)
        {
            int limit  = searchQuery.Limit;
            int offset = searchQuery.Offset;
            var result = new OpenAndClosedTasksMetadata
            {
                OpenTasks   = await dataInterface.GetOpenIncidentTasksMetadataAsync(limit, offset),
                ClosedTasks = await dataInterface.GetClosedIncidentTasksMetadataAsync(limit, offset)
            };

            return Json(result);
        }

        [HttpGet("getIsUniqueTaskNumber/{taskNum}")]
        public async Task<IActionResult> GetIsUniqueTaskNumber(string taskNum)
        {
            bool result = await dataInterface.IsUniqueTaskNumber(taskNum);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncidentTask item)
        {
            var errors = IncidentTaskHelper.ValidateClientTaskProperties(item);
            if (errors != null)
            {
                ModelState.AddModelError(errors.Item1, errors.Item2);
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var errors = IncidentTaskHelper.ValidateClientTaskProperties(item);
            if (errors != null)
            {
                ModelState.AddModelError(errors.Item1, errors.Item2);
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

    public class OpenAndClosedTasksMetadata
    {
        [JsonProperty("openTasks")]
        public PaginationMetadata OpenTasks { get; set; }
        [JsonProperty("closedTasks")]
        public PaginationMetadata ClosedTasks { get; set; }
    }
}
