using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Seeder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class LookupDataTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();
        private EmbcDbContext ctx => Services.ServiceProvider.GetService<EmbcDbContext>();

        public LookupDataTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanListActiveCountries()
        {
            var source = new[]
            {
                new Country{Name="country1", CountryCode="USA", Active=true},
                new Country{Name="country2", CountryCode="CAN", Active=true},
                new Country{Name="country3", CountryCode="IND", Active=false},
                new Country{Name="country4", CountryCode="MEX", Active=true},
            };

            var repo = new SeederRepository(ctx);

            repo.AddOrUpdateCountries(source);

            var result = await di.GetCountriesAsync();
            Assert.Equal(source.Count(c => c.Active), result.Count());
            foreach (var item in result)
            {
                Assert.Contains(item.Id, source.Select(e => e.CountryCode));
            }
        }

        [Fact]
        public async Task CanListActiveCommunities()
        {
            var region = new Region { Name = "region1", Active = true };
            var source = new[]
            {
                new Community{Name="community1", RegionName=region.Name, Active=true},
                new Community{Name="community2", RegionName=region.Name, Active=true},
                new Community{Name="community3", RegionName=region.Name, Active=false},
                new Community{Name="community4", RegionName=region.Name, Active=true},
          };

            var repo = new SeederRepository(ctx);

            repo.AddOrUpdateRegions(new[] { region });
            repo.AddOrUpdateCommunities(source);

            var result = await di.GetCommunitiesAsync();
            Assert.Equal(source.Count(c => c.Active), result.Count());
            foreach (var item in result)
            {
                Assert.Contains(new { name = item.Name, regionName = item.Region.Name }, source.Select(e => new { name = e.Name, regionName = e.RegionName }));
            }
        }

        [Fact]
        public async Task CanListActiveRegions()
        {
            var source = new[]
            {
                new Region {Name="region1", Active=true},
                new Region {Name="region2", Active=true},
                new Region {Name="region3", Active=true},
                new Region {Name="region4", Active=true},
            };

            var repo = new SeederRepository(ctx);

            repo.AddOrUpdateRegions(source);

            var result = await di.GetRegionsAsync();
            Assert.Equal(source.Count(c => c.Active), result.Count());
            foreach (var item in result)
            {
                Assert.Contains(item.Name, source.Select(e => e.Name));
            }
        }

        [Fact]
        public async Task CanListActiveFamilyRelationshipTypes()
        {
            var source = new[]
            {
                new FamilyRelationshipType{Code="FMR1", Active=true},
                new FamilyRelationshipType{Code="FMR2", Active=true},
                new FamilyRelationshipType{Code="FMR3", Active=false},
                new FamilyRelationshipType{Code="FMR4", Active=true},
            };

            var repo = new SeederRepository(ctx);

            repo.AddOrUpdateFamilyRelationshipTypes(source);

            var result = await di.GetFamilyRelationshipTypesAsync();
            Assert.Equal(source.Count(c => c.Active), result.Count());
            foreach (var item in result)
            {
                Assert.Contains(item.Code, source.Select(e => e.Code));
            }
        }
    }
}