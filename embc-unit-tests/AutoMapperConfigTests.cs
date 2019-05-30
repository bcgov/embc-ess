using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class AutoMapperConfigTests : BaseTest
    {
        public AutoMapperConfigTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AssertConfig()
        {
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}