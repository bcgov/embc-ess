using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        private readonly IDataInterface dataInterface;
        private readonly ILogger logger;

        public CountriesController(ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            logger = loggerFactory.CreateLogger(typeof(CountriesController));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return base.Json(await dataInterface.GetCountriesAsync());
        }
    }
}
