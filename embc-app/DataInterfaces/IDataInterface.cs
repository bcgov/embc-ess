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

        #region Incident task

        Task<IEnumerable<IncidentTask>> GetIncidentTasksAsync();

        Task<IncidentTask> GetIncidentTaskAsync(string id);

        Task<IncidentTask> CreateIncidentTaskAsync(IncidentTask task);

        Task UpdateIncidentTaskAsync(IncidentTask task);

        Task<bool> DeactivateIncidentTaskAsync(string id);

        #endregion Incident task

        List<Community> GetCommunities();

        List<Country> GetCountries();

        List<Region> GetRegions();

        List<RegionalDistrict> GetRegionalDistricts();

        List<FamilyRelationshipType> GetFamilyRelationshipTypes();

        #region Organization

        //Organizations
        Task<IEnumerable<Organization>> GetOrganizationsAsync();

        Organization GetOrganizationByLegalName(string name);

        Organization GetOrganizationByExternalId(string externalId);

        Task<Organization> CreateOrganizationAsync(Organization item);

        Task UpdateOrganizationAsync(Organization item);

        Task<bool> DeactivateOrganizationAsync(string id);

        Task<Organization> GetOrganizationAsync(string id);

        #endregion Organization

        #region People

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
