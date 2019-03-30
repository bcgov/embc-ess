using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    public class FamilyRelationshipsController : Controller
    {
        private readonly IDataInterface dataInterface;
        private readonly ILogger logger;

        public FamilyRelationshipsController(ILoggerFactory loggerFactory, IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
            logger = loggerFactory.CreateLogger(typeof(FamilyRelationshipsController));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Json(await dataInterface.GetFamilyRelationshipTypesAsync());
        }
    }
}
