using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface _dataInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private readonly IUrlHelper urlHelper;

        public RegistrationsController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IDataInterface dataInterface,
            IUrlHelper urlHelper
        )
        {
            Configuration = configuration;
            _dataInterface = dataInterface;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(PeopleController));
            this._env = env;
            this.urlHelper = urlHelper;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetAll))]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters queryParameters)
        {
            try
            {
                var results = await _dataInterface.GetRegistrations(queryParameters);

                var toReturn = await PaginatedList<ViewModels.Registration>.CreateAsync(results, queryParameters.Offset, queryParameters.Limit);

                // TODO: provide values for pagination metadata...
                var paginationMetadata = new PaginationMetadata()
                {
                    CurrentPage = toReturn.GetCurrentPage(),
                    PageSize = toReturn.Limit,
                    TotalCount = toReturn.TotalItemCount,
                    TotalPages = toReturn.GetTotalPages()
                };

                return Json(new
                {
                    data = toReturn,
                    metadata = paginationMetadata
                });
            }
            catch (RestException error)
            {
                // TODO: Remove error payload when live in PROD
                return BadRequest(error);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await _dataInterface.GetRegistration(id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] ViewModels.Registration item, string id)
        {
            if (id != null && item.Id != null && id != item.Id)
            {
                return BadRequest();
            }

            return Json(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ViewModels.Registration item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                item.Id = null;
                var result = await _dataInterface.CreateRegistration(item);
                return Json(result);
            }
            catch (RestException error)
            {
                return BadRequest(error);
            }
        }
    }
}
