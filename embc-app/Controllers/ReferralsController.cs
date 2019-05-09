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
        private static int nextId = 1000010;

        private static readonly List<Referral> referrals = new List<Referral>()
            {
                new Referral
                {
                    ReferralId = "D1000001",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier1" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Food",
                    SubType = "Groceries",
                    NumDaysMeals = 3,
                    TotalAmount = 100.00m
                },
                new Referral
                {
                    ReferralId = "D1000002",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier1" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                     Type = "Food",
                    SubType = "Restaurant",
                    NumBreakfasts=3,
                    NumLunches=4,
                    NumDinners=5,
                    TotalAmount = 505.30m
                },
                new Referral
                {
                    ReferralId = "D1000003",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                     Type = "Clothing",
                    SubType = string.Empty,
                    TotalAmount = 100.12m
                },
                new Referral
                {
                    ReferralId = "D1000004",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Incidentals",
                    SubType = (string)null,
                    ApprovedItems = "approved stuff",
                    TotalAmount = 1050.44m
                },
                new Referral
                {
                    ReferralId = "D1000005",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Transportation",
                    SubType = "Taxi",
                    FromAddress = "here",
                    ToAddress = "there",
                    TotalAmount = 200m
                },
                new Referral
                {
                    ReferralId = "D1000006",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Transportation",
                    SubType = "Other",
                    OtherTransportModeDetails = "other transport",
                    TotalAmount = 200m
                },
                new Referral
                {
                    ReferralId = "D1000007",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Lodging",
                    SubType = "Hotel",
                    NumNights = 5,
                    NumRooms = 3,
                    TotalAmount = 200m
                },
                new Referral
                {
                    ReferralId = "D1000008",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Lodging",
                    SubType = "Group",
                    NumNights = 5,
                    TotalAmount = 200m
                },                new Referral
                {
                    ReferralId = "D1000009",
                    Active = true,
                    Supplier = new Supplier { Name = "Supplier2" },
                    ValidFrom = DateTime.Parse("2019-04-02T11:00:00-07:00"),
                    ValidTo = DateTime.Parse("2019-04-06T11:00:00-07:00"),
                    Evacuees = new[]
                    {
                        new Evacuee{Id="1"},
                        new Evacuee{Id="2"},
                        new Evacuee{Id="3"}
                    },
                    Type = "Lodging",
                    SubType = "Billeting",
                    NumNights = 5,
                    TotalAmount = 200m
                }
        };

        [HttpGet]
        public async Task<IActionResult> Get(string registrationId, SearchQueryParameters searchQuery)

        {
            var results = referrals.AsQueryable().Where(r => r.Active).Sort(searchQuery.SortBy ?? "ValidFrom");
            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referrals = new PaginatedList<ReferralListItem>(results.Select(r => r.ToListItem()), searchQuery.Offset, searchQuery.Limit)
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
                referrals.Add(referral);
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
