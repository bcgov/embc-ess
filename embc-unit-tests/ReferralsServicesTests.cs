using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Services.Referrals;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsServicesTests : TestBase
    {
        public ReferralsServicesTests(ITestOutputHelper output) : base(output)
        {
        }

        //[Fact]
        //public async Task CanGetReferralsFromIdCollection()
        //{
        //    var ctx = EmbcDb;

        //    var di = new DataInterface(ctx, mapper);

        //    var referrals = GetReferrals();

        //    var referralIds = new List<string>();
        //    foreach (var referral in referrals)
        //    {
        //        var id = await di.CreateReferralAsync(referral);
        //        referralIds.Add(id);
        //    }

        //    var result = await di.GetReferralsAsync(referralIds);
        //    Assert.Equal(referralIds.Count(), result.Count());
        //}

        [Theory]
        [MemberData(nameof(GetReferrals), "100001")]
        public async Task CanGetReferralHtmlPages(Gov.Jag.Embc.Public.ViewModels.Referral referral)
        {
            var ctx = EmbcDb;
            var di = new DataInterface(ctx, Mapper);
            var pdfService = new PdfConverter();
            var service = new ReferralsService(di, pdfService);

            var incidentTask = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());

            var regVM = RegistrationGenerator.GenerateSelf();
            regVM.IncidentTask = incidentTask;

            var registrationId = await di.CreateEvacueeRegistrationAsync(regVM);

            referral.EssNumber = registrationId;

            var referralIds = new List<string>();
            var id = await di.CreateReferralAsync(referral);
            referralIds.Add(id);

            var printReferrals = new ReferralsToPrint { ReferralIds = referralIds, AddSummary = true };

            var content = await service.GetReferralHtmlPagesAsync(printReferrals);

            Assert.False(string.IsNullOrEmpty(content));
        }

        [Fact]
        public void CanGetAllTemplates()
        {
            foreach (var template in EnumHelper<ReferralsService.ReferalMainViews>.GetValues())
            {
                var result = TemplateLoader.LoadTemplate(template.ToString());
                Assert.NotNull(result);
                Assert.False(string.IsNullOrEmpty(result));
            }

            foreach (var template in EnumHelper<ReferralsService.ReferralPartialView>.GetValues())
            {
                var name = $"{template}.{template}ItemsPartial";
                var result = TemplateLoader.LoadTemplate(name);
                Assert.NotNull(result);

                name = $"{template}.{template}SupplierPartial";
                result = TemplateLoader.LoadTemplate(name);
                Assert.NotNull(result);

                name = $"{template}.{template}ChecklistPartial";
                result = TemplateLoader.LoadTemplate(name);
                Assert.NotNull(result);
                //ChecklistPartial
                //Assert.False(string.IsNullOrEmpty(result));
            }
        }

        [Theory]
        [MemberData(nameof(GetReferrals), "100001")]
        public async Task CanMapToPrintReferrals(Gov.Jag.Embc.Public.ViewModels.Referral referral)
        {
            var ctx = EmbcDb;
            var di = new DataInterface(ctx, Mapper);
            var pdfService = new PdfConverter();
            var service = new ReferralsService(di, pdfService);

            var incidentTask = await di.CreateIncidentTaskAsync(IncidentTaskGenerator.Generate());

            var regVM = RegistrationGenerator.GenerateSelf();
            regVM.IncidentTask = incidentTask;

            var registrationId = await di.CreateEvacueeRegistrationAsync(regVM);

            referral.EssNumber = registrationId;

            var referralId = await di.CreateReferralAsync(referral);
            var id = await di.CreateReferralAsync(referral);

            var result = await di.GetReferralsAsync(new string[] { id });

            Assert.NotNull(result);
            Assert.Equal("D0000001", result.First().IncidentTaskNumber);
            Assert.NotNull(result);
            Assert.Equal(referral.EssNumber, result.First().EssNumber);
            Assert.NotEmpty(result.First().Id);
            Assert.Equal(referral.Type + (referral.SubType != null ? $"_{referral.SubType}" : ""), result.First().Type);
            Assert.Null(result.First().SubType);
            Assert.Equal(referral.Purchaser, result.First().Purchaser);
            Assert.Equal(referral.TotalAmount, result.First().TotalAmount);
            Assert.Equal(referral.Supplier.Fax, result.First().Supplier.Fax);
            Assert.Equal(referral.ValidDates.From, result.First().ValidDates.From);
            Assert.Equal(referral.ValidDates.To, result.First().ValidDates.To);
            Assert.Equal(referral.ConfirmChecked, result.First().ConfirmChecked);
            Assert.All(result.First().Evacuees, e => referral.Evacuees.Any(re => re.Id == e.Id));
            Assert.Equal(referral.Evacuees.Count(), result.First().Evacuees.Count());
            Assert.NotNull(result.First().Supplier);
            Assert.Equal(referral.Supplier.Name, result.First().Supplier.Name);
            Assert.Equal(referral.Supplier.Address, result.First().Supplier.Address);
            Assert.Equal(referral.Supplier.City, result.First().Supplier.City);
            Assert.Equal(referral.Supplier.Province, result.First().Supplier.Province);
            Assert.Equal(referral.Supplier.Fax, result.First().Supplier.Fax);
            Assert.Equal(referral.Supplier.Telephone, result.First().Supplier.Telephone);
            Assert.Equal(referral.Supplier.Active, result.First().Supplier.Active);
            Assert.Equal(referral.ToAddress, result.First().ToAddress);
            Assert.Equal(referral.FromAddress, result.First().FromAddress);
            Assert.Equal(referral.OtherTransportModeDetails, result.First().OtherTransportModeDetails);
            Assert.Equal(referral.NumLunches, result.First().NumLunches);
            Assert.Equal(referral.NumBreakfasts, result.First().NumBreakfasts);
            Assert.Equal(referral.NumDaysMeals, result.First().NumDaysMeals);
            Assert.Equal(referral.NumDinners, result.First().NumDinners);
            Assert.Equal(referral.NumNights, result.First().NumNights);
            Assert.Equal(referral.ApprovedItems, result.First().ApprovedItems);
            Assert.Equal(referral.ExtremeWinterConditions, result.First().ExtremeWinterConditions);
        }

        public static IEnumerable<object[]> GetReferrals(string registrationId)
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

        [Theory]
        [MemberData(nameof(GetReferralTypeTests))]
        public void CanValidateReferralTypes(string type, string subType, bool expectedResult)
        {
            var pdfService = new PdfConverter();
            var svc = new ReferralsService(new DataInterface(EmbcDb, Mapper), pdfService);

            Assert.Equal(expectedResult, svc.IsValidReferralType(type, subType));
        }

        public static IEnumerable<object[]> GetReferralTypeTests()
        {
            var names = Enum.GetNames(typeof(ReferralType));

            var types = names.GroupBy(n => n.Split("_", StringSplitOptions.RemoveEmptyEntries)[0].ToUpperInvariant());
            var allSubTypes = names.Select(n => n.Split("_", StringSplitOptions.RemoveEmptyEntries)).Where(sp => sp.Length > 1).Select(n => n[1].ToUpperInvariant()).Distinct();

            foreach (var type in types)
            {
                if (type.Count() == 1)
                {
                    yield return new object[] { type.Key, null, true };
                    foreach (var subType in allSubTypes)
                    {
                        yield return new object[] { type.Key, subType, false };
                    }
                }
                else
                {
                    var subTypes = type.Select(s => s.Split("_", StringSplitOptions.None)[1].ToUpperInvariant());
                    foreach (var subType in subTypes)
                    {
                        yield return new object[] { type.Key, subType, true };
                    }
                    foreach (var subType in allSubTypes.Except(subTypes))
                    {
                        yield return new object[] { type.Key, subType, false };
                    }
                }
            }
        }
    }
}