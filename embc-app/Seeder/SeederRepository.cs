using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class SeederRepository : ISeederRepository
    {
        readonly EmbcDbContext db;

        public SeederRepository(EmbcDbContext ctx)
        {
            db = ctx;
        }

        public IEnumerable<Country> GetCountries()
        {
            return db.Countries.ToArray();
        }

        public IEnumerable<Region> GetRegions()
        {
            return db.Regions.ToArray();
        }

        public IEnumerable<RegionalDistrict> GetRegionalDistricts()
        {
            return db.RegionalDistricts
                .Include(d => d.Region)
                .ToArray();
        }

        public IEnumerable<Community> GetCommunities()
        {
            return db.Communities
                .Include(c => c.RegionalDistrict)
                    .ThenInclude(d => d.Region)
                .ToArray();
        }

        public void AddCommunities(List<Community> communities)
        {
            db.AddRange(communities);
            db.SaveChanges();
        }

        public void UpdateCommunities(List<Community> communities)
        {
            db.UpdateRange(communities);
            db.SaveChanges();
        }

        public IEnumerable<FamilyRelationshipType> GetFamilyRelationshipTypes()
        {
            return db.FamilyRelationshipTypes.ToArray();
        }
    }
}
