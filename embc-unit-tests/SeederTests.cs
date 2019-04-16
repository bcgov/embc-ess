using System;
using Xunit;
using Gov.Jag.Embc.Public.DataInterfaces;
using NSubstitute;
using Castle.Core.Logging;
using Gov.Jag.Embc.Public.Seeder;
using Gov.Jag.Embc.Public.Models.Db;
using System.Collections.Generic;

namespace embc_unit_tests
{
    public class SeederTests
    {
        [Fact]
        public void GetCommuntiesSeedDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Community>>("Communities");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
