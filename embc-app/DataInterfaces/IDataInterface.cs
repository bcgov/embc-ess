using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        Organization GetOrganizationByBceidGuid(string bceidGuid);

        // Registrations
        Task<IQueryable<Registration>> GetRegistrations(SearchQueryParameters queryParameters);

        Task<Registration> GetRegistration(string id);

        Task<Registration> CreateRegistration(Registration registration);

        // Incident Tasks
        Task<List<IncidentTask>> GetIncidentTasks();

        Task<IncidentTask> GetIncidentTask(string id);

        Task<IncidentTask> CreateIncidentTask(IncidentTask task);

        Task<IncidentTask> UpdateIncidentTask(IncidentTask task);

        List<Community> GetCommunities();

        List<Country> GetCountries();

        List<Region> GetRegions();

        List<RegionalDistrict> GetRegionalDistricts();

        List<FamilyRelationshipType> GetFamilyRelationshipTypes();

        Organization GetOrganizationByLegalName(string name);

        Organization GetOrganizationByExternalId(string externalId);

        Task<Person> GetPersonByBceidGuidAsync(string bceidGuid);

        Task<Volunteer> GetVolunteerByNameAsync(string firstName, string lastName);

        Task<Person> CreatePersonAsync(Person person);

        Task<bool> DeactivatePersonAsync(string id);

        Task<Volunteer> GetVolunteerByExternalIdAsync(string externalId);

        Task<Volunteer> GetVolunteerByIdAsync(string Id);

        Task<IEnumerable<Volunteer>> GetAllVolunteersAsync();
    }
}
