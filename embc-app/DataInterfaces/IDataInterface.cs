using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        #region Registration

        Task<Registration> GetEvacueeRegistrationAsync(string id);

        Task<RegistrationSummary> GetEvacueeRegistrationSummaryAsync(string id);

        Task<string> CreateEvacueeRegistrationAsync(Registration registration);

        Task UpdateEvacueeRegistrationAsync(Registration registration);

        Task<bool> DeactivateEvacueeRegistrationAsync(string id);

        Task AppendEvacueeRegistrationAuditEntryAsync(RegistrationEvent notification, string userId, string userName, string userType);

        Task<IEnumerable<Models.Db.EvacueeRegistrationAudit>> GetEvacueeRegistrationAuditTrailAsync(long essFileNumber);

        #endregion Registration

        #region Incident task

        Task<IPagedResults<IncidentTask>> GetIncidentTasksAsync(SearchQueryParameters searchQuery);

        Task<IncidentTask> GetIncidentTaskAsync(string id);

        Task<IPagedResults<IncidentTask>> GetOpenIncidentTasksAsync(int limit = 100, int offset = 0);

        Task<IncidentTask> GetIncidentTaskByTaskNumbetAsync(string taskNumber);

        Task<string> CreateIncidentTaskAsync(IncidentTask task);

        Task UpdateIncidentTaskAsync(IncidentTask task);

        Task<bool> DeactivateIncidentTaskAsync(string id);

        #endregion Incident task

        #region Lookup data

        Task<IEnumerable<Community>> GetCommunitiesAsync();

        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<Region>> GetRegionsAsync();

        Task<IEnumerable<FamilyRelationshipType>> GetFamilyRelationshipTypesAsync();

        #endregion Lookup data

        #region Organization

        Task<IPagedResults<Organization>> GetOrganizationsAsync(SearchQueryParameters searchQuery);

        Task<Organization> GetOrganizationByBCeIDGuidAsync(string guid);

        Task<string> CreateOrganizationAsync(Organization item);

        Task UpdateOrganizationAsync(Organization item);

        Task<bool> DeactivateOrganizationAsync(string id);

        Task<bool> ActivateOrganizationAsync(string id);

        Task<Organization> GetOrganizationAsync(string id);

        Task<bool> OrganizationExistsAsync(string id);

        #endregion Organization

        #region VolunteerTask

        Task<VolunteerTask> GetVolunteerTaskByIdAsync(int id);
        Task<VolunteerTask> GetVolunteerTaskByIncideTaskIdAsync(Guid taskId);
        Task UpdateVolunteerTasksAsync(VolunteerTask updatedVolunteerTask);
        Task<VolunteerTask> CreateVolunteerTaskAsync(VolunteerTask newVolunteerTask);

        #endregion VolunteerTask

        #region Volunteer

        Task<IPagedResults<Volunteer>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery);

        Task<Volunteer> GetVolunteerByIdAsync(string id);

        Task UpdateVolunteerAsync(Volunteer person);

        Task<string> CreateVolunteerAsync(Volunteer person);

        Task<bool> DeactivateVolunteerAsync(string id);

        Task<bool> ActivateVolunteerAsync(string id);

        Task<Volunteer> GetVolunteerByBceidUserNameAsync(string bceidUserName);

        #endregion Volunteer

        #region Referral

        Task<string> CreateReferralAsync(Referral referral);

        Task<Referral> GetReferralAsync(string referralId);

        Task<IPagedResults<ReferralListItem>> GetReferralsAsync(string registrationId, SearchQueryParameters searchQuery);

        Task<IEnumerable<PrintReferral>> GetReferralsAsync(IEnumerable<string> referralIds);

        Task<bool> DeactivateReferralAsync(string referralId);

        #endregion Referral

        #region Evacuee

        Task<IPagedResults<EvacueeListItem>> GetEvacueesAsync(EvacueeSearchQueryParameters query);

        #endregion Evacuee
    }
}
