using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class RegionalDistrictsController : Controller
    {
        private readonly IDataInterface dataInterface;
        private readonly ILogger logger;

        public RegionalDistrictsController(ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            logger = loggerFactory.CreateLogger(typeof(RegionalDistrictsController));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await dataInterface.GetRegionalDistrictsAsync());
        }
    }
}
