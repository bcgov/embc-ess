using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<List<Organization>> GetOrganizationsAsync()
        {
            var entities = await db.Organizations.ToListAsync();
            var result = new List<Organization>();
            foreach (var item in entities)
            {
                result.Add(item.ToViewModel());
            }
            return result;
        }

        public Organization GetOrganizationByLegalName(string name)
        {
            var item = db.Organizations
                .Include(x => x.Region)
                .Include(x => x.RegionalDistrict)
                .Include(x => x.Community)
                .FirstOrDefault(x => x.Name == name);
            var result = item.ToViewModel();

            return result;
        }

        public Organization GetOrganizationByExternalId(string externalId)
        {
            var item = db.Organizations
                .Include(x => x.Region)
                .Include(x => x.RegionalDistrict)
                .Include(x => x.Community)
                .FirstOrDefault(x => x.Externaluseridentifier == externalId);
            var result = item.ToViewModel();

            return result;
        }

        public async Task<Organization> CreateOrganizationAsync(Organization item)
        {
            var entity = item.ToModel();
            await db.Organizations.AddAsync(entity);
            await db.SaveChangesAsync();

            //Mitigate the EF cascading saving issues
            entity.Region = item.Region.ToModel();
            entity.RegionalDistrict = item.RegionalDistrict.ToModel();
            entity.Community = item.Community.ToModel();
            return entity.ToViewModel();
        }

        public async Task<Organization> UpdateOrganizationAsync(Organization item)
        {
            var entity = await db.Organizations.FirstOrDefaultAsync(x => x.Id == new Guid(item.Id));
            entity.PatchValues(item);
            db.Organizations.Update(entity);
            await db.SaveChangesAsync();

            //Mitigate the EF cascading saving issues
            entity.Region = item.Region.ToModel();
            entity.RegionalDistrict = item.RegionalDistrict.ToModel();
            entity.Community = item.Community.ToModel();
            return entity.ToViewModel();
        }

        public async Task<bool> DeactivateOrganizationAsync(string id)
        {
            var entity = await db.Organizations.FirstOrDefaultAsync(x => x.Id == new Guid(id));
            if (entity == null)
            {
                return false;
            }
            entity.Active = false;
            db.Organizations.Update(entity);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
