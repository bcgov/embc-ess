using Gov.Jag.Embc.Interfaces;

using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Business-User")]
    public class OrganisationsController : Controller
    {
        private readonly BCeIDBusinessQuery _bceid;
        private readonly IConfiguration Configuration;

        //private readonly SharePointFileManager _sharePointFileManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public OrganisationsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, BCeIDBusinessQuery bceid, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _bceid = bceid;
            
            _httpContextAccessor = httpContextAccessor;
            //_sharePointFileManager = sharePointFileManager;
            _logger = loggerFactory.CreateLogger(typeof(OrganisationsController));
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
