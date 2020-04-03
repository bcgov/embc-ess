using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class EvacueesController : Controller
    {
        private readonly IDataInterface dataInterface;

        public EvacueesController(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EvacueeSearchQueryParameters query, string format = "json")
        {
            if (format.Equals("csv", System.StringComparison.OrdinalIgnoreCase))
            {
                var evacuees = await dataInterface.GetEvacueesAsync(query);

                Response.Headers.Add("Content-Disposition", $"inline; filename=\"evacuees_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv\"");

                return Content(evacuees.ToCSV(), "text/csv");
            }
            else
            {
                var evacuees = await dataInterface.GetEvacueesPaginatedAsync(query);
                return Json(evacuees);
            }
        }
    }
}
