using Gov.Jag.Embc.Public.Models.Db;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.Seeder
{
    public interface ISeederRepository
    {
        IEnumerable<Community> GetCommunities();
        void AddCommunities(List<Community> communities);

        void UpdateCommunities(List<Community> communities);

        IEnumerable<Country> GetCountries();

        IEnumerable<Region> GetRegions();

        IEnumerable<RegionalDistrict> GetRegionalDistricts();

        IEnumerable<FamilyRelationshipType> GetFamilyRelationshipTypes();
    }
}
