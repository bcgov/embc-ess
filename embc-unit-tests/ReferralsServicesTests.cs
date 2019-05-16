using Gov.Jag.Embc.Public.Services.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace embc_unit_tests
{
    public class ReferralsServicesTests
    {
        [Fact]
        public void GetCreateHTML()
        {
            var service = new ReferralsService();

            var result = service.GetHtmlContent();
        }
    }
}