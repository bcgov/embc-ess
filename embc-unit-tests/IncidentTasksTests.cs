using AutoFixture;
using AutoMapper;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class IncidentTasksTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();

        public IncidentTasksTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_IncidentTask_Saved()
        {
            var fixture = new Fixture();

            var task = fixture.Build<IncidentTask>()
                .Without(t => t.Id)
                .Without(t => t.Region)
                .With(t => t.Community, await GetRandomSeededCommunity())
                .Create();

            var taskId = await di.CreateIncidentTaskAsync(task);

            var result = await di.GetIncidentTaskAsync(taskId);

            Assert.NotNull(result);
            Assert.Equal(task.Active, result.Active);
            Assert.NotNull(result.Community);
            Assert.Equal(task.Community.Id, result.Community.Id);
            Assert.Null(result.Region);
            Assert.Equal(task.StartDate, result.StartDate);
            Assert.Equal(task.TaskNumber, result.TaskNumber);
            Assert.Equal(0, result.TotalAssociatedEvacuees);
        }

        [Fact]
        public void CanMapNullCorrectly()
        {
            Gov.Jag.Embc.Public.Models.Db.IncidentTask source = new Gov.Jag.Embc.Public.Models.Db.IncidentTask
            {
                Id = Guid.NewGuid(),
                Region = new Gov.Jag.Embc.Public.Models.Db.Region { Active = false, Name = "test" },
                Community = null
            };

            var mapper = Services.ServiceProvider.GetService<IMapper>();
            var executionPlan = mapper.ConfigurationProvider.BuildExecutionPlan(typeof(Gov.Jag.Embc.Public.Models.Db.IncidentTask), typeof(IncidentTask));
            var result = mapper.Map<IncidentTask>(source);

            Assert.Equal(source.Id.ToString(), result.Id);
            Assert.Equal(source.Region.Active, result.Region.Active);
            Assert.Equal(source.Region.Name, result.Region.Name);
            Assert.Null(result.Community);
        }

        [Fact]
        public void CanMapRegionCorrectly()
        {
            var source = new Gov.Jag.Embc.Public.Models.Db.Region { Active = true, Name = "test" };

            var result = Services.ServiceProvider.GetService<IMapper>().Map<Region>(source);

            Assert.Equal(source.Active, result.Active);
            Assert.Equal(source.Name, result.Name);
        }
    }
}