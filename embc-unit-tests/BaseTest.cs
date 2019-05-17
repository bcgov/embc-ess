using AutoMapper;
using Gov.Jag.Embc.Public;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class BaseTest
    {
        private ServiceProvider serviceProvider;

        protected IMapper mapper => serviceProvider.GetService<IMapper>();

        protected EmbcDbContext EmbcDb => serviceProvider.GetService<EmbcDbContext>();

        public BaseTest(ITestOutputHelper output)
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddProvider(new XUnitLoggerProvider(output)))
                .AddAutoMapper(typeof(Startup))
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<EmbcDbContext>(options => options
                    .EnableSensitiveDataLogging()
                    //.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ESS_develop;Integrated Security=True;")
                    .UseInMemoryDatabase("ESS_Test")
                    );

            serviceProvider = services.BuildServiceProvider();
        }

        protected void SeedData()
        {
            var ctx = EmbcDb;

            var repo = new SeederRepository(ctx);

            var types = new[]
            {
                new FamilyRelationshipType{Code="FMR1", Active=true},
                new FamilyRelationshipType{Code="FMR2", Active=true},
                new FamilyRelationshipType{Code="FMR3", Active=false},
                new FamilyRelationshipType{Code="FMR4", Active=true},
            };

            var regions = new[]
            {
                new Region {Name="region1", Active=true},
                new Region {Name="region2", Active=true},
                new Region {Name="region3", Active=true},
                new Region {Name="region4", Active=true},
            };

            var countries = new[]
            {
                new Country{Name="country1", CountryCode="CT1", Active=true},
                new Country{Name="country2", CountryCode="CT2", Active=true},
                new Country{Name="country3", CountryCode="CT3", Active=false},
                new Country{Name="country4", CountryCode="CT4", Active=true},
            };

            var communities = new[]
            {
                new Community{Name="community1", RegionName=regions[0].Name, Active=true},
                new Community{Name="community2", RegionName=regions[0].Name, Active=true},
                new Community{Name="community3", RegionName=regions[0].Name, Active=false},
                new Community{Name="community4", RegionName=regions[0].Name, Active=true},
            };

            repo.AddOrUpdateFamilyRelationshipTypes(types);
            repo.AddOrUpdateCountries(countries);
            repo.AddOrUpdateRegions(regions);
            repo.AddOrUpdateCommunities(communities);
        }
    }
}