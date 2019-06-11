using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels.Search;
using System.Collections.Generic;
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

            var lastName = registration.HeadOfHousehold.LastName.Substring(1, 3);

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = lastName });

            Assert.All(result.Items, e => Assert.Contains(lastName, e.LastName));
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

            Assert.Empty(result.Items);
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

            Assert.All(result.Items, e => Assert.Contains(lastName, e.LastName));
        }

        [Fact]
        public async Task GetAll_BasicSearchByHostCommunity_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var hostCommunityName = toCommunity.Name;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = hostCommunityName });

            Assert.All(result.Items, e => Assert.Contains(hostCommunityName, e.EvacuatedTo));
        }

        [Fact]
        public async Task GetAll_BasicSearchByIncidentCommunity_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var incidentCommunityName = fromCommunity.Name;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = incidentCommunityName });

            Assert.All(result.Items, e => Assert.Contains(incidentCommunityName, e.EvacuatedFrom));
        }

        [Fact]
        public async Task GetAll_BasicSearchByIncidentTaskNumber_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = incidentTaskId });

            Assert.All(result.Items, e => Assert.Equal(incidentTaskId, e.IncidentTaskNumber));
        }

        [Fact]
        public async Task GetAll_BasicSearchByEssFileNumber_MatchedElementReturned()
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var incidentCommunityName = fromCommunity.Name;

            var result = await di.GetEvacueesAsync(new EvacueeSearchQueryParameters() { Query = registrationId });

            Assert.All(result.Items, e => Assert.Equal(registrationId, e.RegistrationId));
        }

        [Theory]
        [MemberData(nameof(GetAdvancedSearchTestCases))]
        public async Task GetAll_AdvancedSearch_MatchedElementReturned(EvacueeSearchQueryParameters search, int expectedNumberOfEvacuees)
        {
            var di = new DataInterface(EmbcDb, Mapper);

            var fromCommunity = await GetRandomSeededCommunity();
            var toCommunity = await GetRandomSeededCommunity();

            var incidentTaskId = await SeedIncident(fromCommunity.Id);
            var registrationId = await di.CreateEvacueeRegistrationAsync(RegistrationGenerator.GenerateCompleted(incidentTaskId, toCommunity.Id));
            var registration = await di.GetEvacueeRegistrationAsync(registrationId);

            var lastName = registration.HeadOfHousehold.FamilyMembers.First().LastName;

            var result = await di.GetEvacueesAsync(search);

            Assert.Equal(expectedNumberOfEvacuees, result.Items.Count());
        }

        public static IEnumerable<object[]> GetAdvancedSearchTestCases()
        {
            var registrations = new List<Gov.Jag.Embc.Public.ViewModels.Registration>();

            return new[] {
                new object[] { new EvacueeSearchQueryParameters { }, 3 },
                //TODO: find a way to test with DB genreated IDs
                //new object[] { new EvacueeSearchQueryParameters { IncidentTaskNumber = ""}, 3 },
                //new object[] { new EvacueeSearchQueryParameters { EssFileNumber = ""}, 3 },
                //new object[] { new EvacueeSearchQueryParameters { EvacuatedFrom = ""}, 3 },
                //new object[] { new EvacueeSearchQueryParameters { EvacuatedFrom = ""}, 3 },
                //new object[] { new EvacueeSearchQueryParameters { EvacuatedTo = ""}, 3 },
                //new object[] { new EvacueeSearchQueryParameters { EvacuatedTo = ""}, 3 },
                new object[] { new EvacueeSearchQueryParameters { HasReferrals = true}, 0 },
                new object[] { new EvacueeSearchQueryParameters { RegistrationCompleted = true}, 3 },
                new object[] { new EvacueeSearchQueryParameters { HasReferrals = false}, 3 },
                new object[] { new EvacueeSearchQueryParameters { RegistrationCompleted = false}, 0 },
                new object[] { new EvacueeSearchQueryParameters { LastName = "hevacueeh" }, 1 },
                new object[] { new EvacueeSearchQueryParameters { LastName = "evacuee" }, 3 },
                new object[] { new EvacueeSearchQueryParameters { FirstName = "fmbr1" }, 1 },
                new object[] { new EvacueeSearchQueryParameters { FirstName = "mbr" }, 2 },
        };
        }
    }
}