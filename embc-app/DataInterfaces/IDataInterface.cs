using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        Organization GetOrganizationByBceidGuid(string bceidGuid);

        Person GetPersonByBceidGuid(string bceidGuid);

        Person CreatePerson(Person person);

        Registration CreateRegistration(Registration registration);

        List<Community> GetCommunities();

        List<Country> GetCountries();

        List<Region> GetRegions();

        List<Registration> GetRegistrations();

        List<RegionalDistrict> GetRegionalDistricts();

    }
}
