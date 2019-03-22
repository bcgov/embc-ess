using Gov.Jag.Embc.Interfaces;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    //[Authorize(Policy = "Business-User")]
    public class OrganizationsController : Controller
    {
        private readonly BCeIDBusinessQuery _bceid;
        private readonly IConfiguration Configuration;

        //private readonly SharePointFileManager _sharePointFileManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IDataInterface _dataInterface;

        public OrganizationsController(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            //BCeIDBusinessQuery bceid, //TODO: Need to restore when BCeIDBusinessQuery is working
            ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            Configuration = configuration;
            //_bceid = bceid;
            
            _httpContextAccessor = httpContextAccessor;
            //_sharePointFileManager = sharePointFileManager;
            _logger = loggerFactory.CreateLogger(typeof(OrganizationsController));
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
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
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
            var result = await Task.FromResult(_dataInterface.GetOrganizationByExternalId(id));

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
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
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

            if (string.IsNullOrWhiteSpace(id))
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
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }


        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            try
            {
                var result = await  _dataInterface.DeactivateOrganizationAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

    }
}
