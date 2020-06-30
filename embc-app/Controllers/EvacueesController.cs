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
        public async Task<IActionResult> Get([FromQuery] EvacueeSearchQueryParameters query, string format = "json")
        {
            if (format.Equals("csv", System.StringComparison.OrdinalIgnoreCase))
            {
                var evacuees = await dataInterface.GetEvacueeReportAsync(query);

                var fileName = $"Evacuees_Export_{ DateTime.Now:yyyyMMdd_HHmmss}.csv";
                return File(Encoding.UTF8.GetBytes(evacuees.ToCSV()), "text/csv;charset=utf-8", fileName);
            }
            else
            {
                var evacuees = await dataInterface.GetEvacueesPaginatedAsync(query);
                return Json(evacuees);
            }
        }

        [HttpGet("getevacueereport")]
        public async Task<IActionResult> EvacueeReport([FromQuery] EvacueeSearchQueryParameters query)
        {
            var evacuees     = await dataInterface.GetEvacueeReportAsync(query);
            TimeZoneInfo pst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, GetPSTTimeZone());
            var fileName     = $"Evacuees_Export_{ today:yyyyMMdd_HHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(evacuees.ToCSV()), "text/csv;charset=utf-8", fileName);
        }

        [HttpGet("getevacueereferralreport")]
        public async Task<IActionResult> EvacueeReferralReport([FromQuery] EvacueeSearchQueryParameters query)
        {
            var evacuees     = await dataInterface.GetEvacueeReferralReportAsync(query);
            TimeZoneInfo pst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, GetPSTTimeZone());
            var fileName     = $"Referral_Export_{ today:yyyyMMdd_HHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(evacuees.ToCSV()), "text/csv;charset=utf-8", fileName);
        }

        private string GetPSTTimeZone()
        {
            return Environment.OSVersion.Platform switch
            {
                PlatformID.Win32NT => "Pacific Standard Time",
                PlatformID.Unix => "Canada/Pacific",
                _ => throw new NotSupportedException()
            };
        }
    }
}
