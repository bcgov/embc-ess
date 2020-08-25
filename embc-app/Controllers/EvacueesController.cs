using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
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
        public async Task<IActionResult> Get([FromQuery] EvacueeSearchQueryParameters query)
        {
            var evacuees = await dataInterface.GetEvacueesPaginatedAsync(query);
            return Json(evacuees);
        }

        [HttpGet("getevacueereport")]
        public async Task<IActionResult> EvacueeReport([FromQuery] EvacueeSearchQueryParameters query)
        {
            var evacuees = await dataInterface.GetEvacueeReportAsync(query);
            var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, GetPSTTimeZone());
            var fileName = $"Evacuees_Export_{ today:yyyyMMdd_HHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(evacuees.ToCSV(query, true, "\"")), "text/csv;charset=utf-8", fileName);
        }

        [HttpGet("getevacueereferralreport")]
        public async Task<IActionResult> EvacueeReferralReport([FromQuery] EvacueeSearchQueryParameters query)
        {
            var evacuees = await dataInterface.GetEvacueeReferralReportAsync(query);
            var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, GetPSTTimeZone());
            var fileName = $"Referral_Export_{ today:yyyyMMdd_HHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(evacuees.ToCSV(query, false, "\"")), "text/csv;charset=utf-8", fileName);
        }

        private string GetPSTTimeZone()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return "Pacific Standard Time";

                case PlatformID.Unix:
                    return "Canada/Pacific";

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
