using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<string> CreateEvacueeRegistrationAsync(Registration registration)
        {
            var created = await db.EvacueeRegistrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();
            //return (await EvacueeRegistrations.SingleAsync(r => r.EssFileNumber == created.Entity.EssFileNumber)).ToViewModel();
            return created.Entity.EssFileNumber.ToString();
        }

        public async Task UpdateEvacueeRegistrationAsync(Registration registration)
        {
            var evacueeRegistration = registration.ToModel();
            var evacueesToKeep = evacueeRegistration.Evacuees.Select(e => e.EvacueeSequenceNumber).ToArray();
            var evacueesToRemove = db.Evacuees
                .Where(e => e.RegistrationId == evacueeRegistration.EssFileNumber && !evacueesToKeep.Contains(e.EvacueeSequenceNumber));

            var addresses = evacueeRegistration.EvacueeRegistrationAddresses.Select(a => a.AddressSequenceNumber).ToArray();
            var mailingAddressToDrop = db.EvacueeRegistrationAddresses
                .SingleOrDefault(a => a.RegistrationId == evacueeRegistration.EssFileNumber && !addresses.Contains(a.AddressSequenceNumber));

            var mailingAddressNeedsToBeUpdated = false;
            if (addresses.Contains(2) && mailingAddressToDrop == null)
            {
                mailingAddressNeedsToBeUpdated = db.EvacueeRegistrationAddresses
                    .Any(e => e.RegistrationId == evacueeRegistration.EssFileNumber && e.AddressSequenceNumber == 2);
            }

            db.EvacueeRegistrations.Update(evacueeRegistration);

            if (mailingAddressNeedsToBeUpdated)
            {
                var mailingAddress = db.EvacueeRegistrationAddresses
                    .Single(e => e.RegistrationId == evacueeRegistration.EssFileNumber && e.AddressSequenceNumber == 2);
                db.Entry(mailingAddress).State = EntityState.Modified;
            }

            db.Evacuees.RemoveRange(evacueesToRemove);
            if (mailingAddressToDrop != null)
            {
                db.EvacueeRegistrationAddresses.Remove(mailingAddressToDrop);
            }

            await db.SaveChangesAsync();
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

        public async Task<bool> DeactivateEvacueeRegistrationAsync(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return false;

            var item = await db.EvacueeRegistrations.SingleOrDefaultAsync(reg => reg.EssFileNumber == essFileNumber);
            if (item == null) return false;
            item.Active = false;
            db.Update(item);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task AppendEvacueeRegistrationAuditEntryAsync(RegistrationEvent notification, string userId)
        {
            await db.EvacueeRegistrationAudits.AddAsync(new Models.Db.EvacueeRegistrationAudit
            {
                User = userId,
                Action = notification.GetType().Name,
                EssFileNumber = long.Parse(notification.EssFileNumber),
                Content = JsonConvert.SerializeObject(notification)
            });
            await db.SaveChangesAsync();
        }
    }
}
