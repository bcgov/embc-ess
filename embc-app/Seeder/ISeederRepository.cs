using Gov.Jag.Embc.Public.Models.Db;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Seeder
{
    public interface ISeederRepository
    {
        IEnumerable<Community> GetCommunities();

        void AddOrUpdateCommunities(IEnumerable<Community> communities);

        IEnumerable<Country> GetCountries();

        IEnumerable<Region> GetRegions();

        void AddOrUpdateRegions(IEnumerable<Region> regions);

        void AddOrUpdateFamilyRelationshipTypes(IEnumerable<FamilyRelationshipType> familyRelationshipTypes);

        void AddOrUpdateIncidentTasks(IEnumerable<IncidentTask> incidentTasks);

        IEnumerable<Organization> GetOrganizations();

        void AddOrUpdateOrganizations(IEnumerable<Organization> organizations);

        void AddOrUpdateVolunteers(IEnumerable<Volunteer> volunteers);

        void AddOrUpdateCountries(IEnumerable<Country> countries);
    }
}
