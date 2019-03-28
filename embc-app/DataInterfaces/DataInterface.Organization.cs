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
        private IQueryable<Models.Db.Organization> Organizations => db.Organizations
                .Include(x => x.Region)
                .Include(x => x.RegionalDistrict)
                .Include(x => x.Community);

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return (await Organizations.ToArrayAsync()).Select(o => o.ToViewModel());
        }

        public Organization GetOrganizationByLegalName(string name)
        {
            return Organizations.FirstOrDefault(x => x.Name == name).ToViewModel();
        }

        public Organization GetOrganizationByExternalId(string externalId)
        {
            return Organizations.FirstOrDefault(x => x.Externaluseridentifier == externalId).ToViewModel();
        }

        public async Task<Organization> GetOrganizationAsync(string id)
        {
            var result = await GetOrganizationInternalAsync(Guid.Parse(id));
            return result?.ToViewModel();
        }

        private async Task<Models.Db.Organization> GetOrganizationInternalAsync(Guid id)
        {
            return await Organizations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Organization> CreateOrganizationAsync(Organization item)
        {
            var newItem = await db.Organizations.AddAsync(item.ToModel());
            await db.SaveChangesAsync();

            return (await GetOrganizationInternalAsync(newItem.Entity.Id)).ToViewModel();
        }

        public async Task UpdateOrganizationAsync(Organization item)
        {
            db.Organizations.Update(item.ToModel());
            await db.SaveChangesAsync();
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
