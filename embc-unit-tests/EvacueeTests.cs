using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels.Search;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class EvacueeTests : TestBase
    {
        public EvacueeTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetAll_All_EvacueePropertiesAreMapped()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters());

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
                Assert.True(e.IsFinalized);
                Assert.False(e.HasReferrals);
            });
        }

        [Fact]
        public async Task GetAll_BasicSearchByHOHLastNameExact_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var lastName = registration.HeadOfHousehold.LastName;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = lastName });

            Assert.All(result.Items, e => Assert.Equal(lastName, e.LastName));
        }

        [Fact]
        public async Task GetAll_BasicSearchByHOHLastNamePartial_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var lastName = registration.HeadOfHousehold.LastName;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = lastName.Substring(1, 3) });

            Assert.All(result.Items, e => Assert.Equal(lastName, e.LastName));
        }

        [Fact]
        public async Task GetAll_BasicSearchByDifferentName_NoMatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var lastName = registration.HeadOfHousehold.LastName;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = "1234" });

            Assert.All(result.Items, e => Assert.Equal(lastName, e.LastName));
        }

        [Fact]
        public async Task GetAll_BasicSearchByFMRLastNamePartial_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var lastName = registration.HeadOfHousehold.FamilyMembers.First().LastName;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = lastName.Substring(1, 3) });

            Assert.All(result.Items, e => Assert.Equal(lastName, e.LastName));
        }
    }
}