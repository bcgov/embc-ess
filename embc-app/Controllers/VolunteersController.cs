using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
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
    public class VolunteersController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;
        private readonly IDataInterface dataInterface;

        public VolunteersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            this.configuration = configuration;

            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(VolunteersController));
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] VolunteersSearchQueryParameters searchQuery)
        {
            var items = await dataInterface.GetVolunteersAsync(searchQuery);

            return Json(new
            {
                data = items.Items,
                metadata = items.Pagination
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await dataInterface.GetVolunteerByIdAsync(id);
            if (item == null) return NotFound(Json(id));
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Volunteer item)
        {
            if (!item.PersonType.Equals(Models.Db.Person.VOLUNTEER, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("PersonType", $"Must be {Models.Db.Person.VOLUNTEER}");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.Id = null;
            item.Active = true;
            var result = await dataInterface.CreateVolunteerAsync(item);
            return Json(await dataInterface.GetVolunteerByIdAsync(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Volunteer item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest(Json(id));
            }
            if (!item.PersonType.Equals(Models.Db.Person.VOLUNTEER, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("PersonType", $"Must be {Models.Db.Person.VOLUNTEER}");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await dataInterface.VolunteerExistsAsync(id))
            {
                return NotFound();
            }
            await dataInterface.UpdateVolunteerAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var result = await dataInterface.DeactivateVolunteerAsync(id);
            return Ok();
        }
    }
}
