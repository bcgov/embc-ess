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
        private IQueryable<Models.Db.Registration> Registrations => db.Registrations
                .Include(reg => reg.HeadOfHousehold)
                    .ThenInclude(hoh => hoh.FamilyMembers)
                        .ThenInclude(fmbr => fmbr.RelationshipToEvacuee)
                .Include(reg => reg.HeadOfHousehold)
                    .ThenInclude(hoh => hoh.PrimaryResidence)
                        .ThenInclude(addr => (addr as Models.Db.BcAddress).Community)
                            .ThenInclude(c => c.RegionalDistrict)
                                .ThenInclude(d => d.Region)
                .Include(reg => reg.HeadOfHousehold)
                    .ThenInclude(hoh => hoh.PrimaryResidence)
                        .ThenInclude(addr => (addr as Models.Db.OtherAddress).Country)
                .Include(reg => reg.HeadOfHousehold)
                    .ThenInclude(hoh => hoh.MailingAddress)
                        .ThenInclude(addr => (addr as Models.Db.BcAddress).Community)
                            .ThenInclude(c => c.RegionalDistrict)
                                .ThenInclude(d => d.Region)
                .Include(reg => reg.HeadOfHousehold)
                    .ThenInclude(hoh => hoh.MailingAddress)
                        .ThenInclude(addr => (addr as Models.Db.OtherAddress).Country)
                .Include(reg => reg.HostCommunity)
                    .ThenInclude(c => c.RegionalDistrict)
                        .ThenInclude(c => c.Region)
                .Include(reg => reg.CompletedBy)
                .Include(reg => reg.IncidentTask)
                    .ThenInclude(t => t.Region)
                .Include(reg => reg.IncidentTask)
                    .ThenInclude(t => t.RegionalDistrict)
                        .ThenInclude(d => d.Region)
                .Include(reg => reg.IncidentTask)
                    .ThenInclude(t => t.Community)
                        .ThenInclude(c => c.RegionalDistrict)
                            .ThenInclude(d => d.Region)
           ;

        public async Task<Registration> CreateRegistrationAsync(Registration registration)
        {
            var created = await db.Registrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();
            return (await Registrations.FirstAsync(r => r.Id == created.Entity.Id)).ToViewModel();
        }

        public async Task UpdateRegistrationAsync(Registration registration)
        {
            db.Registrations.Update(registration.ToModel());
            await db.SaveChangesAsync();
        }

        public async Task<PaginatedList<Registration>> GetRegistrationsAsync(SearchQueryParameters queryParameters)
        {
            var q = queryParameters.Query;

            IQueryable<Models.Db.Registration> registrations = Registrations
                 .Where(r => !queryParameters.HasQuery() ||
                (
                    //TODO: see if it is possible to move this into a method, right now EF refuses to work with lazy loading enabled
                    // and the search criteria in a method, consider switching the search to raw sql for better control of the query
                    r.HeadOfHousehold.LastName.Contains(q, StringComparison.InvariantCultureIgnoreCase) ||
                    r.HeadOfHousehold.FamilyMembers.Any(fm => fm.LastName.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    r.EssFileNumber.ToString().Contains(q, StringComparison.InvariantCultureIgnoreCase) ||
                    (r.IncidentTask != null && r.IncidentTask.TaskNumber.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    (r.HeadOfHousehold.PrimaryResidence is Models.Db.BcAddress) &&
                    ((Models.Db.BcAddress)r.HeadOfHousehold.PrimaryResidence).Community.Name.Contains(q, StringComparison.InvariantCultureIgnoreCase))
                );

            if (queryParameters.HasSortBy())
            {
                // sort using dynamic linq extension method
                registrations = registrations.Sort(queryParameters.SortBy);
            }

            var items = await registrations
                .Skip(queryParameters.Offset)
                .Take(queryParameters.Limit)
                .ToArrayAsync();

            var allItemCount = await registrations.CountAsync();
            return new PaginatedList<Registration>(items.Select(r => r.ToViewModel()), allItemCount, queryParameters.Offset, queryParameters.Limit);
        }

        public async Task<Registration> GetRegistrationAsync(string id)
        {
            var entity = await GetRegistrationInternalAsync(id);
            return entity?.ToViewModel();
        }

        private async Task<Models.Db.Registration> GetRegistrationInternalAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                return await Registrations.FirstOrDefaultAsync(reg => reg.Id == guid);
            }
            return null;
        }

        public async Task<RegistrationSummary> GetRegistrationSummaryAsync(string id)
        {
            var entity = await GetRegistrationInternalAsync(id);
            return entity?.ToSummaryViewModel();
        }

        public async Task<bool> DeactivateRegistration(string id)
        {
            if (!Guid.TryParse(id, out var guid)) return false;

            var item = await db.Registrations.FirstOrDefaultAsync(reg => reg.Id == guid);
            if (item == null) return false;
            item.Active = false;
            db.Update(item);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
