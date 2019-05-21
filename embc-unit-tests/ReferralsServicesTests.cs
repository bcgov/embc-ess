using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
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
    }
}