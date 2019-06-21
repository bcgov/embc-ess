using AutoFixture;
using System;

namespace embc_unit_tests
{
    public class OrganizationCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Gov.Jag.Embc.Public.ViewModels.Organization>(c => c
                .With(o => o.Id, Guid.NewGuid().ToString())
                .Without(o => o.Region)
                .Without(o => o.Community)
                );
        }
    }
}