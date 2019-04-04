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
                .Join(Volunteers.Where(v => v.IsPrimaryContact ?? false), o => o.Id, pc => pc.Organization.Id, (org, pc) => new { org, pc })//Assume a single primary contact
                .ToArrayAsync();

            return new PaginatedList<Organization>(items.Select(i => i.org.ToViewModel(i.pc)), searchQuery.Offset, searchQuery.Limit);
        }

        public Organization GetOrganizationBCeIDGuid(string guid)
        {
            var org = Organizations.FirstOrDefault(x => x.BCeIDBusinessGuid == guid);
            if (org == null) return null;
            return org.ToViewModel(GetPrimaryContactForOrganization(org.Id).GetAwaiter().GetResult());
        }

        public async Task<Organization> GetOrganizationAsync(string id)
        {
            var item = await Organizations.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));
            if (item == null) return null;
            var admin = await GetPrimaryContactForOrganization(item.Id);
            var org = item.ToViewModel(admin);
            org.AdminBCeID = admin.BceidAccountNumber;
            org.AdminFirstName = admin.FirstName;
            org.AdminLastName = admin.LastName;

            return org;
        }

        private async Task<Models.Db.Volunteer> GetPrimaryContactForOrganization(Guid orgId)
        {
            return await Volunteers.SingleAsync(x => x.Organization.Id == orgId && (x.IsPrimaryContact ?? false));
        }

        public async Task<string> CreateOrganizationAsync(Organization item)
        {
            var newItem = await db.Organizations.AddAsync(item.ToModel());
            var admin = new Models.Db.Volunteer()
            {
                Active = true,
                BceidAccountNumber = item.AdminBCeID,
                FirstName = item.AdminFirstName,
                LastName = item.AdminLastName,
                IsAdministrator = true,
                OrganizationId = newItem.Entity.Id,
                IsPrimaryContact = true
            };
            var newAdmin = await db.People.AddAsync(admin);
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
