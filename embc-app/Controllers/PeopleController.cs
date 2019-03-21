using Gov.Jag.Embc.Public.DataInterfaces;
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
    [AllowAnonymous]
    public class PeopleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly IHostingEnvironment env;
        private readonly IDataInterface dataInterface;

        public PeopleController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            this.configuration = configuration;

            this.httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(PeopleController));
            this.env = env;
        }

        [HttpGet("{type}")]
        public async Task<IActionResult> Get(string type)
        {
            try
            {
                return Json(await dataInterface.GetPeopleAsync(type), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, string id)
        {
            try
            {
                return Json(await dataInterface.GetPersonByIdAsync(type, id), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Person item, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || item == null || id != item.Id)
            {
                return BadRequest();
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Person item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                item.Id = null;
                var result = await dataInterface.CreatePersonAsync(item);
                return Json(result, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            try
            {
                var result = await dataInterface.DeactivatePersonAsync(type, id);
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
