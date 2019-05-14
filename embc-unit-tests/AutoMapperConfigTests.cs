using AutoMapper;
using Gov.Jag.Embc.Public;
using Xunit;

namespace embc_unit_tests
{
    public class AutoMapperConfigTests
    {
        [Fact]
        public void AssertConfig()
        {
            Mapper.Initialize(cfg => cfg.AddMaps(typeof(Startup)));
            Mapper.AssertConfigurationIsValid();
        }
    }
}