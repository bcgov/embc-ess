using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private IQueryable<Models.Db.Volunteer> Volunteers => db.Volunteers
            .AsNoTracking()
            .Include(v => v.Organization)
                .ThenInclude(x => x.Region)
            .Include(v => v.Organization)
            .Include(v => v.Organization)
                .ThenInclude(x => x.Community)
                    .ThenInclude(x => x.Region)
        ;

        private IQueryable<Models.Db.Volunteer> ActiveVolunteers => Volunteers.Where(v => v.Active);

        public async Task UpdateVolunteerAsync(Volunteer updatedVolunteer)
        {
            if (string.IsNullOrEmpty(updatedVolunteer.Organization.Id)) throw new InvalidOperationException($"Volunteer {updatedVolunteer.Id} is not associated with an organization");

            var volunteer = updatedVolunteer.ToModel();
            var orgId = volunteer.OrganizationId.Value;
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer, orgId);
                volunteer.IsPrimaryContact = true;
            }
            else
            {
                volunteer.IsPrimaryContact = false;
            }
            db.Volunteers.Update(volunteer);

            await db.SaveChangesAsync();
        }

        private IQueryable<Models.Db.Volunteer> GetVolunteersByOrganizationId(Guid orgId)
        {
            return Volunteers.Where(v => v.Organization.Id == orgId || v.OrganizationId == orgId);
        }

        private async Task SetVolunteerAsPrimaryContact(Models.Db.Volunteer volunteer, Guid organizationId)
        {
            var previousPrimaryContactsForOrg = GetVolunteersByOrganizationId(organizationId).Where(v => v.Id != volunteer.Id && (v.IsPrimaryContact ?? false)).AsTracking();
            foreach (var contact in previousPrimaryContactsForOrg)
            {
                contact.IsPrimaryContact = false;
            }
            db.Volunteers.UpdateRange(previousPrimaryContactsForOrg);
            await Task.CompletedTask;
        }

        private async Task<int> GetNumberOfPrimaryContactsInOrganization(Guid organizationId)
        {
            return await Volunteers.CountAsync(v => (v.Organization.Id == organizationId || v.OrganizationId == organizationId)
                 && v.IsPrimaryContact.HasValue && v.IsPrimaryContact.Value);
        }

        public async Task<IPagedResults<Volunteer>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery)
        {
            var onlyEssUsers = searchQuery.OnlyEssUsers ?? false;
            var onlyAdmins = searchQuery.OnlyAdminUsers ?? false;

            var items = await Volunteers
                 .Where(v => !searchQuery.HasQuery() || EF.Functions.Like(v.LastName, $"%{searchQuery.Query}%"))
                 .Where(v => !onlyEssUsers || !v.IsAdministrator.Value)
                 .Where(v => !onlyAdmins || v.IsAdministrator.Value)
                 .Where(v => searchQuery.OrganizationId == null || v.Organization.Id == Guid.Parse(searchQuery.OrganizationId))
                 .Where(t => searchQuery.Active == t.Active)
                 .Sort(searchQuery.SortBy ?? "lastname")
                 .ToArrayAsync();

            return new PaginatedList<Volunteer>(items.Select(o => o.ToViewModel()), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Volunteer> GetVolunteerByIdAsync(string id)
        {
            var person = await Volunteers.SingleOrDefaultAsync(v => v.Id == Convert.ToInt32(id));
            return person?.ToViewModel();
        }

        public async Task<string> CreateVolunteerAsync(Volunteer newVolunteer)
        {
            if (newVolunteer.Organization == null || string.IsNullOrEmpty(newVolunteer.Organization.Id)) throw new InvalidOperationException($"Volunteer {newVolunteer.Id} is not associated with an organization");

            var volunteer = newVolunteer.ToModel();
            var orgId = volunteer.OrganizationId.Value;
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer, orgId);
                volunteer.IsPrimaryContact = true;
            }
            else
            {
                volunteer.IsPrimaryContact = false;
            }
            var newEntity = await db.Volunteers.AddAsync(volunteer);
            await db.SaveChangesAsync();
            return newEntity.Entity.Id.ToString();
        }

        public async Task<bool> DeactivateVolunteerAsync(string id)
        {
            var volunteer = await db.Volunteers.SingleOrDefaultAsync(v => v.Id == Convert.ToInt32(id));
            if (volunteer == null) return false;

            volunteer.Active = false;
            db.Volunteers.Update(volunteer);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateVolunteerAsync(string id)
        {
            var volunteer = await db.Volunteers.SingleOrDefaultAsync(v => v.Id == Convert.ToInt32(id));
            if (volunteer == null) return false;

            volunteer.Active = true;
            db.Volunteers.Update(volunteer);
            await db.SaveChangesAsync();
            return true;
        }

        public Volunteer GetVolunteerByBceidUserId(string bceidUserId)
        {
            var volunteer = ActiveVolunteers.AsNoTracking().FirstOrDefault(x => x.BceidAccountUserName == bceidUserId);
            if (volunteer == null) return null;

            return volunteer?.ToViewModel();
        }

        public Volunteer GetVolunteerByExternalId(string externalId)
        {
            var volunteer = ActiveVolunteers.AsNoTracking().FirstOrDefault(x => x.BCeId == externalId);
            if (volunteer == null) return null;

            return volunteer?.ToViewModel();
        }
    }
}
