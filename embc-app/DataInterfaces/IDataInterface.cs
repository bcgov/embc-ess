using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        // Registrations
        Task<PaginatedList<Registration>> GetRegistrationsAsync(SearchQueryParameters queryParameters);

        Task<Registration> GetRegistrationAsync(string id);

        Task<RegistrationSummary> GetRegistrationSummaryAsync(string id);

        Task<Registration> CreateRegistrationAsync(Registration registration);

        Task UpdateRegistrationAsync(Registration registration);

        Task<bool> DeactivateRegistration(string id);

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

        #region Organization

        //Organizations
        Task<List<Organization>> GetOrganizationsAsync();

        Task<Organization> GetOrganizationAsync(string id);

        Task<Organization> GetOrganizationByBceidGuidAsync(string bceidGuid);

        Organization GetOrganizationByLegalName(string name);

        Organization GetOrganizationByExternalId(string externalId);

        Task<Organization> CreateOrganizationAsync(Organization item);

        Task<Organization> UpdateOrganizationAsync(Organization item);

        Task<bool> DeactivateOrganizationAsync(string id);

        #endregion Organization



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
