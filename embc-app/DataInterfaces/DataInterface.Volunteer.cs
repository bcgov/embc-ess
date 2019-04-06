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
            if (string.IsNullOrEmpty(person.Organization.Id)) throw new InvalidOperationException($"Volunteer {person.Id} is not associated with an organization");

            var volunteer = (Models.Db.Volunteer)person.ToModel();
            var orgId = volunteer.OrganizationId.Value;
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer, orgId);
            }
            else
            {
                var numberOfPrimaryContacts = await GetNumberOfPrimaryContactsInOrganization(orgId);
                if (numberOfPrimaryContacts <= 1) throw new InvalidOperationException("Organization must have at least 1 primary contact");
            }
            db.People.Update(volunteer);
            await db.SaveChangesAsync();
        }

        private IQueryable<Models.Db.Volunteer> GetVolunteersByOrganizationId(Guid orgId)
        {
            return Volunteers.Where(v => v.Organization.Id == orgId);
        }

        private async Task SetVolunteerAsPrimaryContact(Models.Db.Volunteer volunteer, Guid organizationId)
        {
            var previousPrimaryContactsForOrg = GetVolunteersByOrganizationId(organizationId).Where(v => v.Id != volunteer.Id && (v.IsPrimaryContact ?? false)).AsTracking();
            foreach (var contact in previousPrimaryContactsForOrg)
            {
                contact.IsPrimaryContact = false;
            }
            db.People.UpdateRange(previousPrimaryContactsForOrg);
            volunteer.IsPrimaryContact = true;
            await Task.CompletedTask;
        }

        private async Task<int> GetNumberOfPrimaryContactsInOrganization(Guid organizationId)
        {
            return await Volunteers.CountAsync(v => v.Organization.Id == organizationId && v.IsPrimaryContact.HasValue && v.IsPrimaryContact.Value);
        }

        public async Task<IPagedResults<Volunteer>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery)
        {
            var onlyEssUsers = searchQuery.OnlyEssUsers ?? false;
            var onlyAdmins = searchQuery.OnlyAdminUsers ?? false;
            var items = await Volunteers
                 .Where(v => !searchQuery.HasQuery() || v.LastName.Contains(searchQuery.Query, StringComparison.InvariantCultureIgnoreCase))
                 .Where(v => !onlyEssUsers || !v.IsAdministrator.Value)
                 .Where(v => !onlyAdmins || v.IsAdministrator.Value)
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
            if (string.IsNullOrEmpty(person.Organization.Id)) throw new InvalidOperationException($"Volunteer {person.Id} is not associated with an organization");

            var volunteer = (Models.Db.Volunteer)person.ToModel();
            var orgId = volunteer.OrganizationId.Value;
            if (volunteer.IsPrimaryContact ?? false)
            {
                await SetVolunteerAsPrimaryContact(volunteer, orgId);
            }
            else
            {
                var numberOfPrimaryContacts = await GetNumberOfPrimaryContactsInOrganization(orgId);
                if (numberOfPrimaryContacts <= 1) throw new InvalidOperationException("Organization must have at least 1 primary contact");
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
