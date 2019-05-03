using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/registrations/{registrationId}/[controller]")]
    [Authorize]
    public class ReferralsController : Controller
    {
        private static int nextId = 1000005;

        private static readonly List<ReferralListItem> referrals = new List<ReferralListItem>()
            {
                new ReferralListItem
                {
                    ReferralId = "D1000001",
                    Supplier = new Supplier { Name = "Supplier1" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Type = "Food",
                    SubType = "Groceries",
                    Active = true,
                },
                new ReferralListItem
                {
                    ReferralId = "D1000002",
                    Supplier = new Supplier { Name = "Supplier1" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Type = "Food",
                    SubType = "Groceries",
                    Active = true,
                },
                new ReferralListItem
                {
                    ReferralId = "D1000003",
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Type = "Clothing",
                    SubType = (string)null,
                    Active = true,
                },
                new ReferralListItem
                {
                    ReferralId = "D1000004",
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Type = "Incidentals",
                    SubType = (string)null,
                    Active = true,
                },
            };

        [HttpGet]
        public async Task<IActionResult> Get(string registrationId, SearchQueryParameters searchQuery)
        {
            var results = referrals.AsQueryable().Where(r => r.Active).Sort(searchQuery.SortBy ?? "ValidFrom");
            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referrals = new PaginatedList<ReferralListItem>(results, searchQuery.Offset, searchQuery.Limit)
            }));
        }

        [HttpGet("{referralId}")]
        public async Task<IActionResult> Get(string registrationId, string referralId)
        {
            var result = referrals.AsQueryable().SingleOrDefault(r => r.ReferralId == referralId);
            if (result == null) return NotFound(new
            {
                registrationId = registrationId,
                referralId = referralId
            });

            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referral = result
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string registrationId, [FromBody] IEnumerable<Referral> newReferrals)
        {
            var referralsList = new List<string>();
            foreach (var referral in newReferrals)
            {
                var referralId = ($"D{nextId:D7}");
                referralsList.Add(referralId);
                referrals.Add(new ReferralListItem
                {
                    ReferralId = referralId,
                    Active = true,
                    Type = referral.Type,
                    SubType = referral.SubType,
                    Supplier = referral.Supplier,
                    ValidFrom = referral.ValidFrom,
                    ValidTo = referral.ValidTo
                });
                nextId++;
            }

            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referrals = referralsList.Select(r => new { ReferralId = r }).ToArray()
            }));
        }

        [HttpDelete("{referralId}")]
        public async Task<IActionResult> Delete(string registrationId, string referralId)
        {
            var referral = referrals.SingleOrDefault(r => r.ReferralId == referralId);
            if (referral == null) return NotFound(new
            {
                registrationId = registrationId,
                referralId = referralId
            });
            referral.Active = false;
            return await Task.FromResult(Ok());
        }
    }
}
