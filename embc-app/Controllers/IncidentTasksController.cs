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
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class IncidentTasksController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IDataInterface _dataInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public IncidentTasksController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            IDataInterface dataInterface
        )
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(IncidentTasksController));
            this._env = env;
            _dataInterface = dataInterface;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SearchQueryParameters queryParameters)
        {
            try
            {
                var results = new PaginatedList<IncidentTask>(await _dataInterface.GetIncidentTasks(), 0, queryParameters.Offset, queryParameters.Limit);
                var paginationMetadata = new PaginationMetadata()
                {
                    CurrentPage = results.GetCurrentPage(),
                    PageSize = results.Limit,
                    TotalCount = results.TotalItemCount,
                    TotalPages = results.GetTotalPages()
                };

                return Json(new
                {
                    data = results,
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
            var result = await _dataInterface.GetIncidentTask(id);
            if (result == null)
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
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ViewModels.IncidentTask item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                item.Id = null;
                var result = await _dataInterface.CreateIncidentTask(item);
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
        public async Task<IActionResult> Update([FromBody] ViewModels.IncidentTask item, string id)
        {
            if (id != null && item.Id != null && id == item.Id)
            {
                try
                {
                    var result = await _dataInterface.UpdateIncidentTask(item);
                    return Json(result);
                }
                catch (RestException error)
                {
                    return BadRequest(error);
                }
            }
            return BadRequest();
        }
    }
}
