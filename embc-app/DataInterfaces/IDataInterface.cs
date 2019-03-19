using Gov.Jag.Embc.Public.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        Organization GetOrganizationByBceidGuid(string bceidGuid);

        Person GetPersonByBceidGuid(string bceidGuid);

        Volunteer GetVolunteerByName(string firstName, string lastName);

        Person CreatePerson(Person person);

        // Registrations
        Task<List<Registration>> GetRegistrations();
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

        Volunteer GetVolunteerByExternalId(string externalId);
        Volunteer GetVolunteerById(string Id);

    }
}
