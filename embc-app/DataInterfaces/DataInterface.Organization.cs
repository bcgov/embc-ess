using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private IQueryable<Models.Db.Organization> Organizations => db.Organizations
                .Include(x => x.Region)
                .Include(x => x.RegionalDistrict)
                    .ThenInclude(x => x.Region)
                .Include(x => x.Community)
                    .ThenInclude(x => x.RegionalDistrict)
                        .ThenInclude(x => x.Region);

        public async Task<IPagedResults<Organization>> GetOrganizationsAsync(SearchQueryParameters searchQuery)
        {
            Guid? searchEntityId = searchQuery.HasQuery() ? Guid.Parse(searchQuery.Query) : (Guid?)null;
            var items = await Organizations
                .Where(o => !searchEntityId.HasValue ||
                    o.Community.Id == searchEntityId ||
                    o.RegionalDistrict.Id == searchEntityId ||
                    o.Region.Id == searchEntityId
                )
                .Where(t => searchQuery.IncludeDeactivated || t.Active)
                .Sort(searchQuery.SortBy ?? "id")
                .ToArrayAsync();

            return new PaginatedList<Organization>(items.Select(o => o.ToViewModel()), searchQuery.Offset, searchQuery.Limit);
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
            return await Organizations.SingleOrDefaultAsync(x => x.Id == id);
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
            var entity = await db.Organizations.SingleOrDefaultAsync(x => x.Id == new Guid(id));
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
