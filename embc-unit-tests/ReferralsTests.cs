using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();

        public ReferralsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [MemberData(nameof(GetReferralss), "100001")]
        public async Task CanInsertReferralViewModel(Gov.Jag.Embc.Public.ViewModels.Referral referral)
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            referral.EssNumber = registrationId;

            var referralId = await di.CreateReferralAsync(referral);
            var result = await di.GetReferralAsync(referralId);

            Assert.NotNull(result);
            Assert.Equal(referral.EssNumber, result.EssNumber);
            Assert.NotEmpty(result.Id);
            Assert.Equal(referral.Type.ToUpperInvariant(), result.Type);
            Assert.Equal(referral.SubType?.ToUpperInvariant(), result.SubType);
            Assert.Equal(referral.Purchaser, result.Purchaser);
            Assert.Equal(referral.TotalAmount, result.TotalAmount);
            Assert.Equal(referral.Supplier.Fax, result.Supplier.Fax);
            Assert.Equal(referral.ValidDates.From, result.ValidDates.From);
            Assert.Equal(referral.ValidDates.To, result.ValidDates.To);
            Assert.Equal(referral.ConfirmChecked, result.ConfirmChecked);
            Assert.All(result.Evacuees, e => referral.Evacuees.Any(re => re.Id == e.Id));
            Assert.Equal(referral.Evacuees.Count(), result.Evacuees.Count());
            Assert.NotNull(result.Supplier);
            Assert.Equal(referral.Supplier.Name, result.Supplier.Name);
            Assert.Equal(referral.Supplier.Address, result.Supplier.Address);
            Assert.Equal(referral.Supplier.City, result.Supplier.City);
            Assert.Equal(referral.Supplier.Province, result.Supplier.Province);
            Assert.Equal(referral.Supplier.Fax, result.Supplier.Fax);
            Assert.Equal(referral.Supplier.Telephone, result.Supplier.Telephone);
            Assert.Equal(referral.Supplier.Active, result.Supplier.Active);
            Assert.Equal(referral.ToAddress, result.ToAddress);
            Assert.Equal(referral.FromAddress, result.FromAddress);
            Assert.Equal(referral.OtherTransportModeDetails, result.OtherTransportModeDetails);
            Assert.Equal(referral.NumLunches, result.NumLunches);
            Assert.Equal(referral.NumBreakfasts, result.NumBreakfasts);
            Assert.Equal(referral.NumDaysMeals, result.NumDaysMeals);
            Assert.Equal(referral.NumDinners, result.NumDinners);
            Assert.Equal(referral.NumNights, result.NumNights);
            Assert.Equal(referral.ApprovedItems, result.ApprovedItems);
            Assert.Equal(referral.ExtremeWinterConditions, result.ExtremeWinterConditions);
        }

        [Fact]
        public async Task CanIgnoreExcessiveProperties()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referral = ReferralGenerator.GenerateWithExcessiveProperties(registrationId);
            Assert.NotNull(referral.FromAddress);
            Assert.NotNull(referral.ToAddress);

            var referralId = await di.CreateReferralAsync(referral);
            var result = await di.GetReferralAsync(referralId);

            Assert.Null(result.FromAddress);
            Assert.Null(result.ToAddress);
        }

        [Fact]
        public async Task CanGetAllReferralsForRegistration()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referrals = new[]{
                ReferralGenerator.Generate(ReferralType.Incidentals, registrationId),
                ReferralGenerator.Generate(ReferralType.Clothing, registrationId),
                ReferralGenerator.Generate(ReferralType.Lodging_Group, registrationId),
                ReferralGenerator.Generate(ReferralType.Transportation_Taxi, registrationId),
                ReferralGenerator.Generate(ReferralType.Food_Restaurant, registrationId)
            };
            foreach (var referral in referrals)
            {
                await di.CreateReferralAsync(referral);
            }

            var result = (await di.GetReferralsAsync(registrationId, new SearchQueryParameters())).Items;
            Assert.NotEmpty(result);
            Assert.Equal(referrals.Length, result.Count());
            Assert.All(referrals, r => Assert.Equal(registrationId, r.EssNumber));
        }

        [Fact]
        public async Task CanGetReferralsInOrder()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referrals = new[]{
                ReferralGenerator.Generate(ReferralType.Incidentals, registrationId),
                ReferralGenerator.Generate(ReferralType.Incidentals, registrationId),
                ReferralGenerator.Generate(ReferralType.Clothing, registrationId),
                ReferralGenerator.Generate(ReferralType.Clothing, registrationId),
                ReferralGenerator.Generate(ReferralType.Lodging_Group, registrationId),
                ReferralGenerator.Generate(ReferralType.Lodging_Hotel, registrationId),
                ReferralGenerator.Generate(ReferralType.Transportation_Taxi, registrationId),
                ReferralGenerator.Generate(ReferralType.Food_Restaurant, registrationId),
                ReferralGenerator.Generate(ReferralType.Food_Groceries, registrationId)
            };

            int days = 0;
            foreach (var referral in referrals)
            {
                referral.ValidDates.From = referral.ValidDates.From.AddDays(days);
                referral.ValidDates.To = referral.ValidDates.To.AddDays(days);
                days++;
            }
            var referralTypeOrder = new List<string> { "FOOD", "LODGING", "CLOTHING", "TRANSPORTATION", "INCIDENTALS" };
            var expectedReferralsOrder = referrals
                .OrderBy(r => referralTypeOrder.IndexOf(r.Type.ToUpperInvariant()))
                .ThenByDescending(r => r.ValidDates.From)
                .Select(r => (r.Type.ToUpperInvariant(), r.ValidDates.From))
                .ToArray();

            foreach (var referral in referrals)
            {
                await di.CreateReferralAsync(referral);
            }

            var result = (await di.GetReferralsAsync(registrationId, new SearchQueryParameters())).Items;
            Assert.Equal(expectedReferralsOrder, result.Select(r => (r.Type, r.ValidFrom)).ToArray());
        }

        [Fact]
        public async Task CanDeactivateReferral()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referralId = await di.CreateReferralAsync(ReferralGenerator.Generate(ReferralType.Clothing, registrationId));

            var result = await di.DeactivateReferralAsync(referralId);
            Assert.True(result);

            var referral = await di.GetReferralAsync(referralId);
            Assert.False(referral.Active);
        }

        [Fact]
        public async Task CanQueryReferralsByStatus()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referral1 = ReferralGenerator.Generate(ReferralType.Clothing, registrationId);
            var referral2 = ReferralGenerator.Generate(ReferralType.Clothing, registrationId);
            var referralId1 = await di.CreateReferralAsync(referral1);
            var referralId2 = await di.CreateReferralAsync(referral1);
            await di.DeactivateReferralAsync(referralId2);

            var activeReferrals = await di.GetReferralsAsync(registrationId, new SearchQueryParameters { Active = true });
            var inactiveReferrals = await di.GetReferralsAsync(registrationId, new SearchQueryParameters { Active = false });

            Assert.Single(activeReferrals.Items);
            Assert.True(activeReferrals.Items.First().Active);
            Assert.Single(activeReferrals.Items);
            Assert.False(inactiveReferrals.Items.First().Active);
        }

        [Fact]
        public async Task CanMapToReferralListItem()
        {
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            var referralId = await di.CreateReferralAsync(ReferralGenerator.Generate(ReferralType.Clothing, registrationId));
            var referral = await di.GetReferralAsync(referralId);

            var item = referral.ToListItem();

            Assert.Equal(referral.Id, item.ReferralId);
            Assert.StartsWith("D", item.ReferralId);
            Assert.Equal(referral.SubType, item.SubType);
            Assert.Equal(referral.Type, item.Type);
            Assert.Equal(referral.ValidDates.From, item.ValidFrom);
            Assert.Equal(referral.ValidDates.To, item.ValidTo);
            Assert.Equal(referral.Supplier.Name, item.Supplier.Name);
        }

        public static IEnumerable<object[]> GetReferralss(string registrationId)
        {
            return new[]     {
                new object[] { ReferralGenerator.Generate(ReferralType.Clothing, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Food_Groceries, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Food_Restaurant, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Incidentals, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Billeting, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Group, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Hotel, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Transportation_Other, registrationId) },
                new object[] { ReferralGenerator.Generate(ReferralType.Transportation_Taxi, registrationId) }
            };
        }
    }
}