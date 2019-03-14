using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        Organization GetOrganizationByBceidGuid(string bceidGuid);

        Person GetPersonByBceidGuid(string bceidGuid);

        Person CreatePerson(Person person);

        // Registrations
        Task<List<Registration>> GetRegistrations();
        Task<Registration> GetRegistration(string id);
        Task<Registration> CreateRegistration(Registration registration);

        List<Community> GetCommunities();

        List<Country> GetCountries();

        List<Region> GetRegions();


        List<RegionalDistrict> GetRegionalDistricts();

        List<FamilyRelationshipType> GetFamilyRelationshipTypes();
    }
}
