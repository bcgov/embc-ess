using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsTests : BaseTest
    {
        public ReferralsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [ClassData(typeof(ReferralTestData))]
        public async Task CanInsertReferralViewModel(Gov.Jag.Embc.Public.ViewModels.Referral referral)
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var registration = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateSelf());

            referral.RegistrationId = registration.Id;

            var referralId = await di.CreateReferralAsync(referral);
            var result = await di.GetReferralAsync(referralId);

            Assert.NotNull(result);
            Assert.Equal(referral.RegistrationId, result.RegistrationId);
            Assert.NotEmpty(result.ReferralId);
            Assert.Equal(referral.Type.ToUpperInvariant(), result.Type);
            Assert.Equal(referral.SubType?.ToUpperInvariant(), result.SubType);
            Assert.Equal(referral.Purchaser, result.Purchaser);
            Assert.Equal(referral.TotalAmount, result.TotalAmount);
            Assert.Equal(referral.Supplier.Fax, result.Supplier.Fax);
            Assert.Equal(referral.ValidFrom, result.ValidFrom);
            Assert.Equal(referral.ValidTo, result.ValidTo);
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
            var referral = ReferralGenerator.GenerateWithExcessiveProperties();
            Assert.NotNull(referral.FromAddress);
            Assert.NotNull(referral.ToAddress);

            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var referralId = await di.CreateReferralAsync(referral);
            var result = await di.GetReferralAsync(referralId);

            Assert.Null(result.FromAddress);
            Assert.Null(result.ToAddress);
        }

        [Fact]
        public async Task CanGetAllReferralsForRegistration()
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var registrationId = "100001";

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
            Assert.All(referrals, r => Assert.Equal(registrationId, r.RegistrationId));
        }

        [Fact]
        public async Task CanGetReferralsInOrder()
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var registrationId = "100001";

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
                referral.ValidFrom = referral.ValidFrom.AddDays(days);
                referral.ValidTo = referral.ValidTo.AddDays(days);
                days++;
            }
            var referralTypeOrder = new List<string> { "FOOD", "LODGING", "CLOTHING", "TRANSPORTATION", "INCIDENTALS" };
            var expectedReferralsOrder = referrals
                .OrderBy(r => referralTypeOrder.IndexOf(r.Type.ToUpperInvariant()))
                .ThenByDescending(r => r.ValidFrom)
                .Select(r => (r.Type.ToUpperInvariant(), r.ValidFrom))
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
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var referralId = await di.CreateReferralAsync(ReferralGenerator.Generate(ReferralType.Clothing));

            var result = await di.DeactivateReferralAsync(referralId);
            Assert.True(result);

            var referral = await di.GetReferralAsync(referralId);
            Assert.False(referral.Active);
        }

        [Fact]
        public async Task CanQueryReferralsByStatus()
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var registrationId = "100001";
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
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var referralId = await di.CreateReferralAsync(ReferralGenerator.Generate(ReferralType.Clothing));
            var referral = await di.GetReferralAsync(referralId);

            var item = referral.ToListItem();

            Assert.Equal(referral.ReferralId, item.ReferralId);
            Assert.StartsWith("D", item.ReferralId);
            Assert.Equal(referral.SubType, item.SubType);
            Assert.Equal(referral.Type, item.Type);
            Assert.Equal(referral.ValidFrom, item.ValidFrom);
            Assert.Equal(referral.ValidTo, item.ValidTo);
            Assert.Equal(referral.Supplier.Name, item.Supplier.Name);
        }
    }

    public class ReferralTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Clothing) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Food_Groceries) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Food_Restaurant) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Incidentals) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Billeting) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Group) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Lodging_Hotel) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Transportation_Other) };
            yield return new object[] { ReferralGenerator.Generate(ReferralType.Transportation_Taxi) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}