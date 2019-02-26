using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IConfiguration Configuration;        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public RegistrationsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Configuration = configuration;            
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(PeopleController));
            this._env = env;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {

            return Json(null);
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] ViewModels.Person item, string id)
        {
            if (id != null && item.id != null && id != item.id)
            {
                return BadRequest();
            }

            // get the contact
            Guid contactId = Guid.Parse(id);

            return Json(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] ViewModels.Person item)
        {
            return Json(null);
        }


    }
}
