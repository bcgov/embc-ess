using AutoFixture;
using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class VolunteersTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();

        public VolunteersTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanCreateVolunteer()
        {
            var fixture = new Fixture().Customize(new OrganizationCustomization());

            var org = fixture.Create<Gov.Jag.Embc.Public.ViewModels.Organization>();
            org.Community = await GetRandomSeededCommunity();

            var orgId = await di.CreateOrganizationAsync(org);

            var volunteer = fixture.Build<Gov.Jag.Embc.Public.ViewModels.Volunteer>()
                .With(o => o.Active, true)
                .Without(o => o.Id)
                .With(o => o.Organization, new Gov.Jag.Embc.Public.ViewModels.Organization { Id = orgId })
                .Create();

            var volunteerId = await di.CreateVolunteerAsync(volunteer);

            var result = await di.GetVolunteerByIdAsync(volunteerId);

            Assert.Equal(volunteerId, result.Id);
            Assert.True(result.Active);
            Assert.Equal(volunteer.BceidAccountNumber, result.BceidAccountNumber);
            Assert.Equal(volunteer.CanAccessRestrictedFiles, result.CanAccessRestrictedFiles);
            Assert.Equal(volunteer.Email, result.Email);
            Assert.Equal(volunteer.Externaluseridentifier, result.Externaluseridentifier);
            Assert.Equal(volunteer.IsAdministrator, result.IsAdministrator);
            Assert.Equal(volunteer.IsPrimaryContact, result.IsPrimaryContact);
            Assert.Equal(volunteer.LastName, result.LastName);
            Assert.Equal(volunteer.FirstName, result.FirstName);
            Assert.Equal(volunteer.Organization?.Id, result.Organization?.Id);
        }
    }
}