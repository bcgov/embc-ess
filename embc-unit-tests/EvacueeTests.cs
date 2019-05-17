using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class EvacueeTests : BaseTest
    {
        public EvacueeTests(ITestOutputHelper output) : base(output)
        {
            SeedData();
        }

        [Fact]
        public async Task CanMapToListItem()
        {
            var ctx = EmbcDb;
            var di = new DataInterface(ctx, mapper);

            var fromCommunity = (await di.GetCommunitiesAsync()).First();
            var toCommunity = (await di.GetCommunitiesAsync()).Last();

            var incidentTask = await di.CreateIncidentTaskAsync(new Gov.Jag.Embc.Public.ViewModels.IncidentTask()
            {
                Active = true,
                Community = new Gov.Jag.Embc.Public.ViewModels.Community { Id = fromCommunity.Id }
            });
            var registration = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTask.Id, toCommunity.Id));

            var result = await di.GetEvacueesAsync(new SearchQueryParameters());

            Assert.Equal(registration.HeadOfHousehold.FamilyMembers.Count(), result.Items.Count(e => !e.IsHeadOfHousehold));
            Assert.Equal(1, result.Items.Count(e => e.IsHeadOfHousehold));
            Assert.All(result.Items, e =>
            {
                Assert.Equal(registration.IncidentTask.TaskNumber, e.IncidentTaskNumber);
                Assert.Equal(registration.Id, e.RegistrationId);
                Assert.Equal(registration.RestrictedAccess, e.RestrictedAccess);
                Assert.Equal(registration.RegistrationCompletionDate, e.RegistrationCompletionDate);
                Assert.Equal(registration.IncidentTask.Community.Name, e.EvacuatedFrom);
                Assert.Equal(registration.HostCommunity.Name, e.EvacuatedTo);
            });
        }
    }
}