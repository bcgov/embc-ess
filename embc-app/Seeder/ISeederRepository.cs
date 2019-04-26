using Gov.Jag.Embc.Public.Models.Db;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Seeder
{
    public interface ISeederRepository
    {
        IEnumerable<Community> GetCommunities();
        void AddOrUpdateCommunities(List<Community> communities);

        IEnumerable<Country> GetCountries();

        IEnumerable<Region> GetRegions();

        void AddOrUpdateRegions(List<Region> regions);
        
        void AddOrUpdateFamilyRelationshipTypes(List<FamilyRelationshipType> familyRelationshipTypes);

        void AddOrUpdateIncidentTasks(List<IncidentTask> incidentTasks);

        IEnumerable<Organization> GetOrganizations();

        void AddOrUpdateOrganizations(List<Organization> organizations);

        void AddOrUpdateVolunteers(List<Volunteer> volunteers);

        void AddOrUpdateCountries(List<Country> countries);
    }
}
