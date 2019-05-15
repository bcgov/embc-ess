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
            .AsNoTracking()
            .Include(x => x.Region)
            .Include(x => x.Community)
                .ThenInclude(x => x.Region);

        public async Task<IPagedResults<Organization>> GetOrganizationsAsync(SearchQueryParameters searchQuery)
        {
            Guid? communityId = null;
            string regionName = null;
            if (searchQuery.HasQuery() && Guid.TryParse(searchQuery.Query, out Guid searchEntityId))
            {
                communityId = searchEntityId;
            }
            else if (searchQuery.HasQuery())
            {
                regionName = searchQuery.Query;
            }
            var items = await Organizations
                .Where(o => (!communityId.HasValue || o.Community.Id == communityId) ||
                    (string.IsNullOrEmpty(regionName) || o.RegionName.Equals(regionName, StringComparison.OrdinalIgnoreCase))
                )
                .Where(t => searchQuery.Active == t.Active)
                .Sort(searchQuery.SortBy ?? "id")
                .Join(Volunteers.Where(v => v.IsPrimaryContact ?? false), o => o.Id, pc => pc.Organization.Id, (org, pc) => new { org, pc })//Assume a single primary contact
                .ToArrayAsync();

            return new PaginatedList<Organization>(items.Select(i => i.org.ToViewModel(i.pc)), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Organization> GetOrganizationByBCeIDGuidAsync(string guid)
        {
            var org = await Organizations.FirstOrDefaultAsync(x => x.BCeIDBusinessGuid == guid);
            if (org == null) return null;
            return org.ToViewModel(GetPrimaryContactForOrganization(org.Id).GetAwaiter().GetResult());
        }

        public async Task<Organization> GetOrganizationAsync(string id)
        {
            var item = await Organizations.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (item == null) return null;
            var admin = await GetPrimaryContactForOrganization(item.Id);
            var org = item.ToViewModel(admin);

            return org;
        }

        public async Task<bool> OrganizationExistsAsync(string id)
        {
            return await Organizations.AnyAsync(x => x.Id == Guid.Parse(id));
        }

        private async Task<Models.Db.Volunteer> GetPrimaryContactForOrganization(Guid orgId)
        {
            return await Volunteers.FirstOrDefaultAsync(x => x.Organization.Id == orgId && (x.IsPrimaryContact ?? false));
        }

        public async Task<string> CreateOrganizationAsync(Organization item)
        {
            var newItem = await db.Organizations.AddAsync(item.ToModel());
            var admin = new Models.Db.Volunteer()
            {
                Active = true,
                BceidAccountUserName = item.AdminBCeID,
                FirstName = item.AdminFirstName,
                LastName = item.AdminLastName,
                IsAdministrator = true,
                OrganizationId = newItem.Entity.Id,
                IsPrimaryContact = true
            };
            var newAdmin = await db.Volunteers.AddAsync(admin);
            await db.SaveChangesAsync();

            return newItem.Entity.Id.ToString();
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

        public async Task<bool> ActivateOrganizationAsync(string id)
        {
            var entity = await db.Organizations.SingleOrDefaultAsync(x => x.Id == new Guid(id));
            if (entity == null)
            {
                return false;
            }
            entity.Active = true;
            db.Organizations.Update(entity);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
