using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        #region Registration

        Task<IPagedResults<Registration>> GetRegistrationsAsync(SearchQueryParameters searchQuery);

        Task<Registration> GetRegistrationAsync(string id);

        Task<RegistrationSummary> GetRegistrationSummaryAsync(string id);

        Task<Registration> CreateRegistrationAsync(Registration registration);

        Task UpdateRegistrationAsync(Registration registration);

        Task<bool> DeactivateRegistration(string id);

        #endregion Registration

        #region Incident task

        Task<IPagedResults<IncidentTask>> GetIncidentTasksAsync(SearchQueryParameters searchQuery);

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
        Task<IPagedResults<Organization>> GetOrganizationsAsync(SearchQueryParameters searchQuery);

        Organization GetOrganizationByLegalName(string name);

        Organization GetOrganizationByExternalId(string externalId);

        Task<Organization> CreateOrganizationAsync(Organization item);

        Task UpdateOrganizationAsync(Organization item);

        Task<bool> DeactivateOrganizationAsync(string id);

        Task<Organization> GetOrganizationAsync(string id);

        #endregion Organization

        #region People

        Task<IPagedResults<Person>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery);

        Task<Person> GetPersonByIdAsync(string id);

        Task UpdatePersonAsync(Person person);

        Task<Person> CreatePersonAsync(Person person);

        Task<bool> DeactivatePersonAsync(string id);

        #endregion People

        #region Volunteer

        Volunteer GetVolunteerByName(string firstName, string lastName);

        Volunteer GetVolunteerByExternalId(string externalId);

        Volunteer GetVolunteerByBceidUserId(string bceidUserId);

        Volunteer GetVolunteerById(string Id);

        #endregion Volunteer
    }
}
