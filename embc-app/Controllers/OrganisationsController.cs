using Gov.Jag.Embc.Interfaces;

using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
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
        private readonly IDataInterface _dataInterface;

        public OrganisationsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, BCeIDBusinessQuery bceid, ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            Configuration = configuration;
            _bceid = bceid;
            
            _httpContextAccessor = httpContextAccessor;
            //_sharePointFileManager = sharePointFileManager;
            _logger = loggerFactory.CreateLogger(typeof(OrganisationsController));
            _dataInterface = dataInterface;
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ViewModels.Organization> result = await _dataInterface.GetOrganizationsAsync();
                return Json(result);
            }
            catch (RestException error)
            {
                return BadRequest(error);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = Task.FromResult(_dataInterface.GetOrganizationByExternalId(id));

            if(result == null)
            {
                return NotFound();
            }

            return Json(result);
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Organization item)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                item.Id = null;
                var result = await _dataInterface.CreateOrganizationAsync(item);
                return Json(result);
            }
            catch (RestException error)
            {
                return BadRequest(error);
            }
        }



        /// <summary>
        /// Update
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] ViewModels.Organization item, string id)
        {
            if (id != null && item.Id != null && id != item.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _dataInterface.UpdateOrganizationAsync(item);
                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (RestException error)
            {
                return BadRequest(error);
            }
        }

    }
}
