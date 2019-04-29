using Xunit;
using Gov.Jag.Embc.Public.Seeder;
using Gov.Jag.Embc.Public.Models.Db;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace embc_unit_tests
{
    public class SeederTests
    {
        private SeedDataLoader seedDataLoader;

        public SeederTests()
        {
            var loggerFactory = Substitute.For<ILoggerFactory>();
            this.seedDataLoader = new SeedDataLoader(loggerFactory);
        }

        [Fact]
        public void GetCommuntySeedDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<Community>>("Communities");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetCountrySeedDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<Country>>("Countries");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.True(result.TrueForAll(c => !string.IsNullOrEmpty(c.CountryCode)));
        }

        [Fact]
        public void GetFamilyRelationshipTypesSeedDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<FamilyRelationshipType>>("FamilyRelationshipTypes");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetIncidentTasksSeederDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<IncidentTask>>("IncidentTasks");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetOrganizationsSeederDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<Organization>>("Organizations");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }


        [Fact]
        public void GetRegionsSeederDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<Region>>("Regions");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetVolunteersSeederDataFromSeedDataLoader()
        {
            var result = seedDataLoader.GetSeedData<List<Volunteer>>("Volunteers");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
