using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private IQueryable<Models.Db.EvacueeRegistration> EvacueeRegistrations => db.EvacueeRegistrations
            .AsNoTracking()
            .Include(ireg => ireg.Evacuees)
            .Include(ireg => ireg.EvacueeRegistrationAddresses)
                .ThenInclude(ireg => ireg.Community)
                    .ThenInclude(d => d.Region)
            .Include(ireg => ireg.EvacueeRegistrationAddresses)
                .ThenInclude(ireg => ireg.Country)
            .Include(reg => reg.HostCommunity)
                .ThenInclude(c => c.Region)
            .Include(reg => reg.IncidentTask)
                .ThenInclude(t => t.Region)
            .Include(reg => reg.IncidentTask)
                .ThenInclude(d => d.Region)
            .Include(reg => reg.IncidentTask)
                .ThenInclude(t => t.Community)
                    .ThenInclude(d => d.Region);

        public async Task<Registration> CreateEvacueeRegistrationAsync(Registration registration)
        {
            var created = await db.EvacueeRegistrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();
            return (await EvacueeRegistrations.SingleAsync(r => r.EssFileNumber == created.Entity.EssFileNumber)).ToViewModel();
        }

        public async Task UpdateEvacueeRegistrationAsync(Registration registration)
        {
            var evacueeRegistration = registration.ToModel();
            var evacueesToKeep = evacueeRegistration.Evacuees.Select(e => e.EvacueeSequenceNumber).ToArray();
            var evacueesToRemove = db.Evacuees
                .Where(e => e.RegistrationId == evacueeRegistration.EssFileNumber && !evacueesToKeep.Contains(e.EvacueeSequenceNumber));

            db.EvacueeRegistrations.Update(evacueeRegistration);
            db.Evacuees.RemoveRange(evacueesToRemove);

            await db.SaveChangesAsync();
        }

        public async Task<IPagedResults<Registration>> GetEvacueeRegistrationsAsync(SearchQueryParameters searchQuery)
        {
            var q = searchQuery.Query;

            var items = await EvacueeRegistrations
                .Where(r => !searchQuery.HasQuery() ||
                    r.Evacuees.Any(e => e.LastName.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    r.EssFileNumber.ToString().Contains(q, StringComparison.InvariantCultureIgnoreCase) ||
                    (r.IncidentTask != null && r.IncidentTask.TaskNumber.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    r.EvacueeRegistrationAddresses.Any(a =>
                        a.AddressSubType == AddressSubType.BCAddress && a.AddressType == AddressType.Primary &&
                        a.Community.Name.Contains(q, StringComparison.InvariantCultureIgnoreCase)))
                    .Where(t => searchQuery.IncludeDeactivated || t.Active)
                .Sort(searchQuery.SortBy ?? "EssFileNumber")
                .ToArrayAsync();

            return new PaginatedList<Registration>(items.Select(r => r.ToViewModel()), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Registration> GetEvacueeRegistrationAsync(string id)
        {
            var entity = await GetEvacueeRegistrationInternalAsync(id);
            return entity?.ToViewModel();
        }

        private async Task<Models.Db.EvacueeRegistration> GetEvacueeRegistrationInternalAsync(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return null;

            return await EvacueeRegistrations.SingleOrDefaultAsync(reg => reg.EssFileNumber == essFileNumber);
        }

        public async Task<RegistrationSummary> GetEvacueeRegistrationSummaryAsync(string id)
        {
            var entity = await GetEvacueeRegistrationInternalAsync(id);
            return entity?.ToSummaryViewModel();
        }

        public async Task<bool> DeactivateEvacueeRegistration(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return false;

            var item = await db.EvacueeRegistrations.SingleOrDefaultAsync(reg => reg.EssFileNumber == essFileNumber);
            if (item == null) return false;
            item.Active = false;
            db.Update(item);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
