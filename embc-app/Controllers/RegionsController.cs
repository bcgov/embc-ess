using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Sqlite.Models;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class RegionsController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface _dataInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public RegionsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env, IDataInterface dataInterface)
        {
            Configuration = configuration;
            _dataInterface = dataInterface;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(PeopleController));
            this._env = env;
        }

        [HttpGet()]
        [AllowAnonymous]
        public IActionResult Get(string id)
        {
            List<ViewModels.Region> result = _dataInterface.GetRegions();

            /*
            

            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "Vancouver Island"
            });

            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "South West"
            });

            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "Central"
            });

            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "Central"
            });


            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "South East"
            });


            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "North East"
            });

            result.Add(new Region()
            {
                Id = Guid.NewGuid(),
                Name = "North West"
            });
            */

            return Json(result);
            
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
