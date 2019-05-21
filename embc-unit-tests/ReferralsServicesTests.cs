using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Services.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class ReferralsServicesTests : BaseTest
    {
        public ReferralsServicesTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanGetReferralsFromIdCollection()
        {
            var ctx = EmbcDb;

            var di = new DataInterface(ctx, mapper);

            var referrals = GetReferrals();

            var referralIds = new List<string>();
            foreach (var referral in referrals)
            {
                var id = await di.CreateReferralAsync(referral);
                referralIds.Add(id);
            }

            var result = await di.GetReferralsAsync(referralIds);
            Assert.Equal(referralIds.Count(), result.Count());
        }

        //[Fact]
        //public async Task CanGetReferralHtmlPages()
        //{
        //    var ctx = EmbcDb;

        //    var di = new DataInterface(ctx, mapper);

        //    var service = new ReferralsService(di);
        //    var referrals = GetReferrals().Take(1);

        //    var referralIds = new List<string>();
        //    foreach (var referral in referrals)
        //    {
        //        var id = await di.CreateReferralAsync(referral);
        //        referralIds.Add(id);
        //    }

        //    //var result = service.CreateReferralHtmlContent();
        //    var content = await service.GetReferralHtmlPages(referralIds);

        //    //var pdfConverter = new PdfConverter();
        //    //await pdfConverter.ConvertHtmlToPdfAsync(content);
        //    Assert.False(string.IsNullOrEmpty(content));
        //}

        private IEnumerable<Gov.Jag.Embc.Public.ViewModels.Referral> GetReferrals()
        {
            var registrationId = "100001";

            var referrals = new[]{
                ReferralGenerator.Generate(ReferralType.Incidentals, registrationId),
                ReferralGenerator.Generate(ReferralType.Clothing, registrationId),
                ReferralGenerator.Generate(ReferralType.Lodging_Group, registrationId),
                ReferralGenerator.Generate(ReferralType.Lodging_Hotel, registrationId),
                ReferralGenerator.Generate(ReferralType.Transportation_Taxi, registrationId),
                ReferralGenerator.Generate(ReferralType.Food_Restaurant, registrationId),
                ReferralGenerator.Generate(ReferralType.Food_Groceries, registrationId)
            };

            return referrals;
        }

        [Theory]
        [MemberData(nameof(GetReferralTypeTests))]
        public void CanValidateReferralTypes(string type, string subType, bool expectedResult)
        {
            var svc = new ReferralsService(new DataInterface(EmbcDb, mapper));

            Assert.Equal(expectedResult, svc.IsValidReferralType(type, subType));
        }

        public static IEnumerable<object[]> GetReferralTypeTests()
        {
            var names = Enum.GetNames(typeof(ReferralType));

            var types = names.GroupBy(n => n.Split("_", StringSplitOptions.RemoveEmptyEntries)[0]);
            var allSubTypes = names.Select(n => n.Split("_", StringSplitOptions.RemoveEmptyEntries)).Where(sp => sp.Length > 1).Select(n => n[1]).Distinct();

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
                    var subTypes = type.Select(s => s.Split("_", StringSplitOptions.None)[1]);
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