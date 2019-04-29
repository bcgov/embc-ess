using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class SystemusersController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public SystemusersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory.CreateLogger(typeof(SystemusersController));
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
