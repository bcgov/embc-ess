using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/registrations/*/referrals")]
    [Authorize]
    public class ReferralsController : Controller
    {
        [HttpGet("{registrationId}/referrals")]
        public async Task<IActionResult> Get(string registrationId, [FromQuery] SearchQueryParameters searchQuery)
        {
            var referrals = new[]{
                new
                {
                    ReferralId = "D1000001",
                    Supplier = new {Name="Supplier1" },
                    Valid = new {From = DateTimeOffset.Parse("2019-04-02"), To = DateTimeOffset.Parse("2019-04-06")},
                    Type = "Food",
                    SubType = "Groceries",
                    Active = true,
                },
                new
                {
                    ReferralId = "D1000002",
                    Supplier = new {Name="Supplier1" },
                    Valid = new {From = DateTimeOffset.Parse("2019-04-02"), To = DateTimeOffset.Parse("2019-04-06")},
                    Type = "Food",
                    SubType = "Groceries",
                    Active = true,
                },
                new
                {
                    ReferralId = "D1000003",
                    Supplier =  new {Name="Supplier2" },
                    Valid = new {From = DateTimeOffset.Parse("2019-04-02"), To = DateTimeOffset.Parse("2019-04-06")},
                    Type = "Clothing",
                    SubType = string.Empty,
                    Active = true,
                },
                new
                {
                    ReferralId = "D1000004",
                    Supplier =  new {Name="Supplier2" },
                    Valid = new {From = DateTimeOffset.Parse("2019-04-02"), To = DateTimeOffset.Parse("2019-04-06")},
                    Type = "Incidentals",
                    SubType = string.Empty,
                    Active = true,
                },
            };

            var results = referrals.AsQueryable().Sort(searchQuery.SortBy ?? "ReferralId");
            return await Task.FromResult(Json(new { RegistrationId = registrationId, Referrals = results }));
        }
    }
}
