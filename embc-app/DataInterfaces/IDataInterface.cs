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

        #region Lookup data

        Task<IEnumerable<Community>> GetCommunitiesAsync();

        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<Region>> GetRegionsAsync();

        Task<IEnumerable<RegionalDistrict>> GetRegionalDistrictsAsync();

        Task<IEnumerable<FamilyRelationshipType>> GetFamilyRelationshipTypesAsync();

        #endregion Lookup data

        #region Organization

        Task<IPagedResults<Organization>> GetOrganizationsAsync(SearchQueryParameters searchQuery);

        Organization GetOrganizationBCeIDGuid(string guid);

        Task<string> CreateOrganizationAsync(Organization item);

        Task UpdateOrganizationAsync(Organization item);

        Task<bool> DeactivateOrganizationAsync(string id);

        Task<bool> ActivateOrganizationAsync(string id);

        Task<Organization> GetOrganizationAsync(string id);

        Task<bool> OrganizationExistsAsync(string id);

        #endregion Organization

        #region Volunteer

        Task<IPagedResults<Volunteer>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery);

        Task<Volunteer> GetVolunteerByIdAsync(string id);

        Task UpdateVolunteerAsync(Volunteer person);

        Task<string> CreateVolunteerAsync(Volunteer person);

        Task<bool> DeactivateVolunteerAsync(string id);

        Task<bool> ActivateVolunteerAsync(string id);

        Volunteer GetVolunteerByName(string firstName, string lastName);

        Volunteer GetVolunteerByExternalId(string externalId);

        Volunteer GetVolunteerByBceidUserId(string bceidUserId);

        Task<bool> VolunteerExistsAsync(string id);

        #endregion Volunteer
    }
}
