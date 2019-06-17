using AutoMapper;
using FakeItEasy;
using Gov.Jag.Embc.Public;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Seeder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class TestBase
    {
        private ServiceProvider serviceProvider;

        protected IMapper Mapper => serviceProvider.CreateScope().ServiceProvider.GetService<IMapper>();

        protected EmbcDbContext EmbcDb => serviceProvider.CreateScope().ServiceProvider.GetService<EmbcDbContext>();

        protected IMediator Mediator => serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>();

        public TestBase(ITestOutputHelper output)
        {
            var configuration = A.Fake<IConfiguration>();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(output));

            var services = new ServiceCollection();

            //This must be called before Startup.ConfigureServices to ensure the in memory db context
            //is registered first, otherwise the tests will try to connect to SQL server
            services
               .AddEntityFrameworkInMemoryDatabase()
               .AddDbContext<EmbcDbContext>(options => options
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .UseInMemoryDatabase("ESS_Test")
                  );

            new Startup(configuration, loggerFactory).ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();

            ViewModelConversions.mapper = serviceProvider.GetService<IMapper>();
            SeedData();
        }

        private void SeedData()
        {
            if (!EmbcDb.Database.IsInMemory()) return;
            var repo = new SeederRepository(EmbcDb);

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
                new Country{Name="country1", CountryCode="USA", Active=true},
                new Country{Name="country2", CountryCode="CAN", Active=true},
                new Country{Name="country3", CountryCode="IND", Active=false},
                new Country{Name="country4", CountryCode="MEX", Active=true},
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

        protected async Task<string> SeedIncident(string communityId)
        {
            var di = new DataInterface(EmbcDb, Mapper);
            var task = IncidentTaskGenerator.Generate();
            task.Community = new Gov.Jag.Embc.Public.ViewModels.Community() { Id = communityId };

            var incidentTask = await di.CreateIncidentTaskAsync(task);
            return incidentTask.Id;
        }

        protected async Task<string[]> SeedRegistrations(string taskId, string hostCommunity, int numberOfRegistrations)
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var registrations = new List<string>();
            for (int i = 0; i < numberOfRegistrations; i++)
            {
                var registration = RegistrationGenerator.GenerateCompleted(taskId, hostCommunity);
                registrations.Add(await di.CreateEvacueeRegistrationAsync(registration));
            }

            return registrations.ToArray();
        }

        protected async Task<Gov.Jag.Embc.Public.ViewModels.Community> GetRandomSeededCommunity()
        {
            var di = new DataInterface(EmbcDb, Mapper);
            var rnd = new Random();
            var communities = (await di.GetCommunitiesAsync()).ToArray();
            return communities.ElementAt(Math.Abs(rnd.Next(communities.Length - 1)));
        }
    }
}