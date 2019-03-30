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

            try
            {
                item.Id = null;
                item.Active = true;
                var result = await dataInterface.CreateOrganizationAsync(item);
                return Json(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ViewModels.Organization item, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            if (!item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Id", "id does not match Organization Id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await dataInterface.UpdateOrganizationAsync(item);
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
                var result = await dataInterface.DeactivateOrganizationAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
