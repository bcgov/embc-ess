using Gov.Jag.Embc.Public.Services.Registrations;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            var created = await db.EvacueeRegistrations.AddAsync(mapper.Map<Models.Db.EvacueeRegistration>(registration));
            await db.SaveChangesAsync();
            return created.Entity.EssFileNumber.ToString();
        }

        public async Task UpdateEvacueeRegistrationAsync(Registration registration)
        {
            using (var tx = await db.Database.BeginTransactionAsync())
            {
                var evacueeRegistration = mapper.Map<Models.Db.EvacueeRegistration>(registration);
                db.Evacuees.RemoveRange(db.Evacuees.Where(e => e.RegistrationId == registration.EssFileNumber));
                db.EvacueeRegistrationAddresses.RemoveRange(db.EvacueeRegistrationAddresses.Where(e => e.RegistrationId == registration.EssFileNumber));

                foreach (var evacuee in evacueeRegistration.Evacuees)
                {
                    db.Entry(evacuee).State = EntityState.Added;
                }

                foreach (var address in evacueeRegistration.EvacueeRegistrationAddresses)
                {
                    db.Entry(address).State = EntityState.Added;
                }

                db.Update(evacueeRegistration);

                await db.SaveChangesAsync();
                tx.Commit();
            }
        }

        public async Task<Registration> GetEvacueeRegistrationAsync(string id)
        {
            var entity = await GetEvacueeRegistrationInternalAsync(id);
            return mapper.Map<Registration>(entity);
        }

        private async Task<Models.Db.EvacueeRegistration> GetEvacueeRegistrationInternalAsync(string id)
        {
            if (!long.TryParse(id, out var essFileNumber)) return null;

            return await EvacueeRegistrations.SingleOrDefaultAsync(reg => reg.EssFileNumber == essFileNumber);
        }

        public async Task<RegistrationSummary> GetEvacueeRegistrationSummaryAsync(string id)
        {
            var entity = await GetEvacueeRegistrationAsync(id);
            return mapper.Map<RegistrationSummary>(entity);
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

        public async Task AppendEvacueeRegistrationAuditEntryAsync(RegistrationEvent notification, string userId, string userName, string userType)
        {
            await db.EvacueeRegistrationAudits.AddAsync(new Models.Db.EvacueeRegistrationAudit
            {
                User = userId,
                UserName = userName,
                UserType = userType,
                Action = notification.GetType().Name,
                EssFileNumber = long.Parse(notification.EssFileNumber),
                Content = JsonConvert.SerializeObject(notification)
            });
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.Db.EvacueeRegistrationAudit>> GetEvacueeRegistrationAuditTrailAsync(long essFileNumber)
        {
            return await db.EvacueeRegistrationAudits
                .Where(e => e.EssFileNumber == essFileNumber)
                .OrderByDescending(e => e.Date)
                .ToArrayAsync();
        }
    }
}
