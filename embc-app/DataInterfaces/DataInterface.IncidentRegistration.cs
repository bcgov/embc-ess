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
        private IQueryable<Models.Db.IncidentRegistration> IncidentRegistrations => db.IncidentRegistrations
            .AsNoTracking()
            .Include(ireg => ireg.Evacuees)
            .Include(ireg => ireg.IncidentRegistrationAddresses);



        public async Task<Registration> CreateIncidentRegistrationAsync(Registration registration)
        {
            var created = await db.IncidentRegistrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();
            return (await IncidentRegistrations.SingleAsync(r => r.Id == created.Entity.Id)).ToViewModel();
        }

        public async Task UpdateIncidentRegistrationAsync(Registration registration)
        {
            var incidentRegistration = registration.ToModel();
            var evacueesToKeep = incidentRegistration.Evacuees.Select(e => e.EvacueeSequenceNumber).ToArray();
            var evacueesToRemove = db.Evacuees
                .Where(e => e.IncidentRegistrationId == incidentRegistration.Id && !evacueesToKeep.Contains(e.EvacueeSequenceNumber));

            db.IncidentRegistrations.Update(incidentRegistration);
            db.Evacuees.RemoveRange(evacueesToRemove);

            await db.SaveChangesAsync();
        }

        public async Task<IPagedResults<Registration>> GetIncidentRegistrationsAsync(SearchQueryParameters searchQuery)
        {
            var q = searchQuery.Query;

            var items = await IncidentRegistrations
                .Where(r => !searchQuery.HasQuery() ||
                    r.Evacuees.Any(e => e.LastName.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    r.EssFileNumber.ToString().Contains(q, StringComparison.InvariantCultureIgnoreCase) ||
                    (r.IncidentTask != null && r.IncidentTask.TaskNumber.Contains(q, StringComparison.InvariantCultureIgnoreCase)) ||
                    r.IncidentRegistrationAddresses.Any(a =>
                        a.AddressSubType == AddressSubType.BCAddress && a.AddressType == AddressType.Primary &&
                        a.Community.Name.Contains(q, StringComparison.InvariantCultureIgnoreCase)))
                    .Where(t => searchQuery.IncludeDeactivated || t.Active)
                .Sort(searchQuery.SortBy ?? "id")
                .ToArrayAsync();

            return new PaginatedList<Registration>(items.Select(r => r.ToViewModel()), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Registration> GetIncidentRegistrationAsync(string id)
        {
            var entity = await GetIncidentRegistrationInternalAsync(id);
            return entity?.ToViewModel();
        }


        private async Task<Models.Db.IncidentRegistration> GetIncidentRegistrationInternalAsync(string id)
        {
            return await IncidentRegistrations.SingleOrDefaultAsync(reg => reg.Id == Guid.Parse(id));
        }

        public async Task<RegistrationSummary> GetIncidentRegistrationSummaryAsync(string id)
        {
            var entity = await GetIncidentRegistrationInternalAsync(id);
            return entity?.ToSummaryViewModel();
        }

        public async Task<bool> DeactivateIncidentRegistration(string id)
        {
            if (!Guid.TryParse(id, out var guid)) return false;

            var item = await db.IncidentRegistrations.SingleOrDefaultAsync(reg => reg.Id == guid);
            if (item == null) return false;
            item.Active = false;
            db.Update(item);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
