using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsFixture : BaseTest
    {
        public ReferralsFixture(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanInsertNewClothingReferral()
        {
            var ctx = EmbcDb;

            var registrationID = 100001L;

            var referral = new ClothingReferral()
            {
                Comments = "comments",
                ExtremeWeatherConditions = true,
                RegistrationId = registrationID,
                ValidFrom = DateTime.Parse("2019-02-28T03:30:44"),
                ValidTo = DateTime.Parse("2019-03-04T11:00:00"),
                Supplier = new Supplier()
                {
                    Name = "supplier1",
                    Address = "address",
                    City = "vancity"
                },
                Evacuees = new[]
                {
                    new ReferralEvacuee{ RegistrationId = registrationID, EvacueeId=1, IsPurchaser=true },
                    new ReferralEvacuee{ RegistrationId = registrationID, EvacueeId=2, IsPurchaser=false }
                },
                TotalAmount = 100.23m,
                ConfirmChecked = true
            };

            ctx.Add(referral);
            ctx.SaveChanges();
        }

        [Theory]
        [ClassData(typeof(ReferralTestData))]
        public async Task CanInsertReferralViewModel(Gov.Jag.Embc.Public.ViewModels.Referral referral)
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var result = await di.CreateReferral(referral);

            Assert.NotNull(result);
            Assert.Equal(referral.RegistrationId, result.RegistrationId);
            Assert.NotEmpty(result.ReferralId);
            Assert.Equal(referral.Type, result.Type);
            Assert.Equal(referral.SubType, result.SubType);
            Assert.Equal(referral.Purchaser, result.Purchaser);
            Assert.Equal(referral.TotalAmount, result.TotalAmount);
            Assert.Equal(referral.Supplier.Fax, result.Supplier.Fax);
            Assert.Equal(referral.ValidFrom, result.ValidFrom);
            Assert.Equal(referral.ValidTo, result.ValidTo);
            Assert.Equal(referral.Purchaser, result.Purchaser);
            Assert.Equal(referral.ConfirmChecked, result.ConfirmChecked);
            foreach (var evacuee in referral.Evacuees)
            {
                Assert.Contains(result.Evacuees, e => e.Id == evacuee.Id);
            }
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

            var result = await di.CreateReferral(referral);

            Assert.Null(result.FromAddress);
            Assert.Null(result.ToAddress);
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

    public class ReferralGenerator
    {
        private static Func<Gov.Jag.Embc.Public.ViewModels.Referral> baseReferral = () => new Gov.Jag.Embc.Public.ViewModels.Referral
        {
            Active = true,
            TotalAmount = 124m,
            Comments = "comments",
            Purchaser = "1",
            Evacuees = new[]
                {
                    new Gov.Jag.Embc.Public.ViewModels.ReferralEvacuee{ Id="2" }
            },
            ConfirmChecked = true,
            RegistrationId = "100001",
            Supplier = new Gov.Jag.Embc.Public.ViewModels.Supplier
            {
                Active = true,
                Name = "supplier1",
                Address = "test",
                City = "city",
                Province = "prov",
                PostalCode = "12334",
                Fax = "112325",
                Telephone = "54356"
            },
            ValidFrom = DateTime.Parse("2019-01-01T11:00"),
            ValidTo = DateTime.Parse("2019-01-05T11:00")
        };

        public static Gov.Jag.Embc.Public.ViewModels.Referral Generate(ReferralType type)
        {
            var referral = baseReferral();
            switch (type)
            {
                case ReferralType.Food_Groceries:
                    referral.Type = "Food";
                    referral.SubType = "Groceries";
                    referral.NumDaysMeals = 3;
                    break;

                case ReferralType.Food_Restaurant:
                    referral.Type = "Food";
                    referral.SubType = "Restaurant";
                    referral.NumBreakfasts = 4;
                    referral.NumLunches = 5;
                    referral.NumDinners = 6;
                    break;

                case ReferralType.Transportation_Taxi:
                    referral.Type = "Transportation";
                    referral.SubType = "Taxi";
                    referral.FromAddress = "here";
                    referral.ToAddress = "there";
                    break;

                case ReferralType.Transportation_Other:
                    referral.Type = "Transportation";
                    referral.SubType = "Other";
                    referral.OtherTransportModeDetails = "skytrain and seabus";
                    break;

                case ReferralType.Lodging_Hotel:
                    referral.Type = "Lodging";
                    referral.SubType = "Billeting";
                    referral.NumNights = 5;
                    referral.NumRooms = 2;
                    break;

                case ReferralType.Lodging_Group:
                    referral.Type = "Lodging";
                    referral.SubType = "Group";
                    referral.NumNights = 5;
                    break;

                case ReferralType.Lodging_Billeting:
                    referral.Type = "Lodging";
                    referral.SubType = "Billeting";
                    referral.NumNights = 5;
                    break;

                case ReferralType.Incidentals:
                    referral.Type = "Incidentals";
                    referral.SubType = null;
                    referral.ApprovedItems = "approved items";
                    break;

                case ReferralType.Clothing:
                    referral.Type = "Clothing";
                    referral.SubType = null;
                    referral.ExtremeWinterConditions = true;
                    break;
            }

            return referral;
        }

        public static Gov.Jag.Embc.Public.ViewModels.Referral GenerateWithExcessiveProperties()
        {
            var referral = Generate(ReferralType.Transportation_Other);
            referral.FromAddress = "from";
            referral.ToAddress = "to";

            return referral;
        }
    }
}