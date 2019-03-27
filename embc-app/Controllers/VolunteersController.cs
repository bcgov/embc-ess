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
    [AllowAnonymous]
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
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters searchQuery)
        {
            try
            {
                var items = await dataInterface.GetPeopleAsync(searchQuery);

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
                return Json(await dataInterface.GetPersonByIdAsync(id));
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
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
            try
            {
                item.Id = null;
                var result = await dataInterface.CreatePersonAsync(item);
                return Json(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Volunteer item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest();
            }
            if (!item.PersonType.Equals(Models.Db.Person.VOLUNTEER, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("PersonType", $"Must be {Models.Db.Person.VOLUNTEER}");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await dataInterface.UpdatePersonAsync(item);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            try
            {
                var result = await dataInterface.DeactivatePersonAsync(id);
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
