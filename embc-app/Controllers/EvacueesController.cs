using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    public class EvacueesController : Controller
    {
        private readonly IDataInterface dataInterface;

        public EvacueesController(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<IActionResult> Get(SearchQueryParameters query)
        {
            var evacuees = await dataInterface.GetEvacueesAsync(query);
            return Json(evacuees);
        }
    }
}
