using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return (await db.Countries.ToArrayAsync()).Select(c => c.ToViewModel());
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync()
        {
            return (await db.Regions.ToArrayAsync()).Select(r => r.ToViewModel());
        }

        public async Task<IEnumerable<RegionalDistrict>> GetRegionalDistrictsAsync()
        {
            return (await db.RegionalDistricts
                .Include(d => d.Region)
                .ToArrayAsync()).Select(d => d.ToViewModel());
        }

        public async Task<IEnumerable<Community>> GetCommunitiesAsync()
        {
            return (await db.Communities
                .Include(c => c.RegionalDistrict)
                    .ThenInclude(d => d.Region)
                .ToArrayAsync())
                .OrderBy(c => c.Name)
                .Select(d => d.ToViewModel());
        }

        public async Task<IEnumerable<FamilyRelationshipType>> GetFamilyRelationshipTypesAsync()
        {
            return (await db.FamilyRelationshipTypes.ToArrayAsync()).Select(t => t.ToViewModel());
        }
    }
}
