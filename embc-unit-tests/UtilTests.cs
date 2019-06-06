using Gov.Jag.Embc.Public.Utils;
using System;
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
            Assert.Equal("May-29-2019", result);
        }

        [Fact]
        public void CanGetLocalTime12h24h()
        {
            var utcTime = new DateTime(2019, 6, 1, 0, 30, 0);
            Assert.Equal("5:30 PM", TimeZoneConverter.GetLocalTime(utcTime));
            Assert.Equal("17:30", TimeZoneConverter.GetLocalTime24h(utcTime));
        }
    }
}