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
            try
            {
                var items = await dataInterface.GetIncidentTasksAsync(searchQuery);

                return Json(new
                {
                    data = items.Items,
                    metadata = items.Pagination
                });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await dataInterface.GetIncidentTaskAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Json(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncidentTask item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                item.Id = null;
                item.Active = true;
                var result = await dataInterface.CreateIncidentTaskAsync(item);
                return Json(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
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

            try
            {
                await dataInterface.UpdateIncidentTaskAsync(item);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            try
            {
                var result = await dataInterface.DeactivateIncidentTaskAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
    }
}
