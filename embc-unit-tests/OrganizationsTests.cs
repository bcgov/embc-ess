﻿using AutoFixture;
using Gov.Jag.Embc.Public.DataInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace embc_unit_tests
{
    public class OrganizationsTests : TestBase
    {
        private IDataInterface di => Services.ServiceProvider.GetService<IDataInterface>();

        public OrganizationsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanCreateOrganization()
        {
            var fixture = new Fixture().Customize(new OrganizationCustomization());

            var org = fixture.Create<Gov.Jag.Embc.Public.ViewModels.Organization>();
            org.Community = await GetRandomSeededCommunity();

            var orgId = await di.CreateOrganizationAsync(org);

            var result = await di.GetOrganizationAsync(orgId);

            Assert.Equal(orgId, result.Id);
            Assert.Equal(org.Name, result.Name);
            Assert.True(result.Active);
            Assert.Equal(org.AdminBCeID, result.AdminBCeID);
            Assert.Equal(org.AdminFirstName, result.AdminFirstName);
            Assert.Equal(org.AdminLastName, result.AdminLastName);
            Assert.Equal(org.BCeIDBusinessGuid, result.BCeIDBusinessGuid);
            Assert.Equal(org.Community?.Id, result.Community?.Id);
            Assert.Equal(org.Region?.Name, result.Region?.Name);
        }
    }
}