using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    //[Authorize(Policy = "Business-User")]
    public class OrganizationsController : Controller
    {
        private readonly BCeIDBusinessQuery _bceid;

        //private readonly SharePointFileManager _sharePointFileManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly ILogger logger;
        private readonly IDataInterface dataInterface;

        public OrganizationsController(
            IHttpContextAccessor httpContextAccessor,
            //BCeIDBusinessQuery bceid, //TODO: Need to restore when BCeIDBusinessQuery is working
            ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            //_bceid = bceid;

            this.httpContextAccessor = httpContextAccessor;
            //_sharePointFileManager = sharePointFileManager;
            logger = loggerFactory.CreateLogger(typeof(OrganizationsController));
            this.dataInterface = dataInterface;
        }

        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters searchQuery)
        {
            try
            {
                var items = await dataInterface.GetOrganizationsAsync(searchQuery);

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
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await dataInterface.GetOrganizationAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpPost()]
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
