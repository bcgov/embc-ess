using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class UtilTests : BaseTest
    {
        public UtilTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvertDateToPST()
        {
            var utc9hourAhead = new DateTime(2019, 5, 30, 1, 0, 0);
            var result = TimeZoneConverter.GetLocalDate(utc9hourAhead);
            var expected = new DateTime(2019, 5, 29, 16, 0, 0).ToString("MMM-dd-yyyy");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanGetLocalTime12h24h()
        {
            var dt = new DateTime(2019, 6, 1, 17, 30, 0);
            Assert.Equal(TimeZoneConverter.GetLocalTime(dt), "5:30 PM");
            Assert.Equal(TimeZoneConverter.GetLocalTime24h(dt), "17:30");
        }
    }
}