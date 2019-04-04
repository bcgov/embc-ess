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
        private IQueryable<Models.Db.Volunteer> Volunteers => db.People
            .AsNoTracking()
            .Where(p => p is Models.Db.Volunteer)
            .Cast<Models.Db.Volunteer>()
            .Include(v => v.Organization)
                .ThenInclude(x => x.Region)
            .Include(v => v.Organization)
                .ThenInclude(x => x.RegionalDistrict)
            .Include(v => v.Organization)
                .ThenInclude(x => x.Community)
                    .ThenInclude(x => x.RegionalDistrict)
                        .ThenInclude(x => x.Region)
        ;

        public async Task UpdateVolunteerAsync(Volunteer person)
        {
            var volunteer = (Models.Db.Volunteer)person.ToModel();
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer);
            }
            db.People.Update(volunteer);
            await db.SaveChangesAsync();
        }

        private IQueryable<Models.Db.Volunteer> GetVolunteersByOrganizationId(Guid orgId)
        {
            return Volunteers.Where(v => v.Organization.Id == orgId);
        }

        private async Task SetVolunteerAsPrimaryContact(Models.Db.Volunteer volunteer)
        {
            var previousPrimaryContactsForOrg = GetVolunteersByOrganizationId(volunteer.Organization.Id).Where(v => v.IsPrimaryContact ?? false);
            await previousPrimaryContactsForOrg.ForEachAsync(v => v.IsPrimaryContact = false);
            db.People.UpdateRange(previousPrimaryContactsForOrg);
            volunteer.IsPrimaryContact = true;
        }

        public async Task<IPagedResults<Volunteer>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery)
        {
            var items = await Volunteers
                 .Where(v => !searchQuery.HasQuery() || v.LastName.Contains(searchQuery.Query, StringComparison.InvariantCultureIgnoreCase))
                 .Where(v => !searchQuery.OnlyEssUsers.HasValue || v.IsAdministrator != searchQuery.OnlyEssUsers.Value)
                 .Where(v => !searchQuery.OnlyAdminUsers.HasValue || v.IsAdministrator == searchQuery.OnlyAdminUsers.Value)
                 .Where(v => searchQuery.OrganizationId == null || v.Organization.Id == Guid.Parse(searchQuery.OrganizationId))
                 .Where(t => searchQuery.IncludeDeactivated || t.Active)
                 .Sort(searchQuery.SortBy ?? "id")
                 .ToArrayAsync();

            return new PaginatedList<Volunteer>(items.Select(o => o.ToViewModel()), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Volunteer> GetVolunteerByIdAsync(string id)
        {
            var person = await Volunteers.SingleOrDefaultAsync(v => v.Id == Guid.Parse(id));
            return person?.ToViewModel();
        }

        public async Task<bool> VolunteerExistsAsync(string id)
        {
            return await Volunteers.AnyAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task<string> CreateVolunteerAsync(Volunteer person)
        {
            var volunteer = (Models.Db.Volunteer)person.ToModel();
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer);
            }
            var newPerson = await db.People.AddAsync(volunteer);
            await db.SaveChangesAsync();
            return newPerson.Entity.Id.ToString();
        }

        public async Task<bool> DeactivateVolunteerAsync(string id)
        {
            var person = await db.People.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id)) as Models.Db.Volunteer;
            if (person == null) return false;

            person.Active = false;
            db.People.Update(person);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateVolunteerAsync(string id)
        {
            var person = await db.People.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id)) as Models.Db.Volunteer;
            if (person == null) return false;

            person.Active = true;
            db.People.Update(person);
            await db.SaveChangesAsync();
            return true;
        }

        public Volunteer GetVolunteerByBceidUserId(string bceidUserId)
        {
            var volunteer = Volunteers.AsNoTracking().FirstOrDefault(x => x.BceidAccountNumber == bceidUserId);
            if (volunteer == null) return null;

            return volunteer.ToViewModel();
        }

        public Volunteer GetVolunteerByExternalId(string externalId)
        {
            var volunteer = Volunteers.FirstOrDefault(x => x.Externaluseridentifier == externalId);
            if (volunteer == null) return null;

            return volunteer.ToViewModel();
        }

        public Volunteer GetVolunteerByName(string firstName, string lastName)
        {
            var volunteer = Volunteers.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (volunteer == null) return null;

            return volunteer.ToViewModel();
        }
    }
}
