using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class RegionsController : Controller
    {
        private readonly IDataInterface dataInterface;
        private readonly ILogger logger;

        public RegionsController(ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            logger = loggerFactory.CreateLogger(typeof(RegionsController));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await dataInterface.GetRegionsAsync());
        }
    }
}
