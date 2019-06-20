using AutoFixture;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
            Assert.Equal(task.Community?.Id, result.Community?.Id);
            Assert.Equal(task.Region?.Name, result.Region?.Name);
            Assert.Equal(task.StartDate, result.StartDate);
            Assert.Equal(task.TaskNumber, result.TaskNumber);
            Assert.Equal(0, result.TotalAssociatedEvacuees);
        }
    }
}