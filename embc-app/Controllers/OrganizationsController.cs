using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly ILogger logger;
        private readonly IDataInterface dataInterface;

        public OrganizationsController(
            ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            logger = loggerFactory.CreateLogger(typeof(OrganizationsController));
            this.dataInterface = dataInterface;
        }

        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters searchQuery)
        {
            var items = await dataInterface.GetOrganizationsAsync(searchQuery);

            return Json(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await dataInterface.GetOrganizationAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Organization item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.Id = null;
            item.Active = true;
            var orgId = await dataInterface.CreateOrganizationAsync(item);

            return Json(await dataInterface.GetOrganizationAsync(orgId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Organization item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest(Json(id));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await dataInterface.OrganizationExistsAsync(id))
            {
                return NotFound();
            }

            await dataInterface.UpdateOrganizationAsync(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var result = await dataInterface.DeactivateOrganizationAsync(id);
            return Ok();
        }
    }
}
