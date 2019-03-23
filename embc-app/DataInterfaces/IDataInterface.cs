using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {                

        // Registrations
        Task<IQueryable<Registration>> GetRegistrations(SearchQueryParameters queryParameters);

        Task<Registration> GetRegistration(string id);

        Task<Registration> CreateRegistration(Registration registration);

        Task<Registration> UpdateRegistration(Registration registration);

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

        Task<Organization> CreateOrganizationAsync(Organization organization);

        #region People
        Person GetPersonByBceidGuid(string bceidGuid);

        Person CreatePerson(Person person);

        Task<IEnumerable<Person>> GetPeopleAsync(string type);

        Task<Person> GetPersonByIdAsync(string type, string id);

        Task UpdatePersonAsync(Person person);

        Task<Person> CreatePersonAsync(Person person);

        Task<bool> DeactivatePersonAsync(string type, string id);

        #endregion People

        #region Volunteer

        Volunteer GetVolunteerByName(string firstName, string lastName);

        Volunteer GetVolunteerByExternalId(string externalId);

        Volunteer GetVolunteerByBceidUserId(string bceidUserId);

        Volunteer GetVolunteerById(string Id);

        #endregion Volunteer

    }
}
