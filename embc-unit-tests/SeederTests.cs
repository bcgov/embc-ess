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
        public void GetCommuntySeedDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Community>>("Communities");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetCountrySeedDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Country>>("Countries");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetFamilyRelationshipTypesSeedDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<FamilyRelationshipType>>("FamilyRelationshipTypes");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetIncidentTasksSeederDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<IncidentTask>>("IncidentTasks");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetOrganizationsSeederDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Organization>>("Organizations");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetRegionalDistrictsSeederDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<RegionalDistrict>>("RegionalDistricts");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetRegionsSeederDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Region>>("Regions");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetVolunteersSeederDataFromSeedDataLoader()
        {
            var result = SeedDataLoader.GetSeedData<List<Volunteer>>("Volunteers");

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
