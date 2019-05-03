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
        private IQueryable<Models.Db.IncidentRegistration> IncidentRegistrations => db.IncidentRegistrations
            .AsNoTracking()
            .Include(ireg => ireg.Evacuees);


        public async Task<Registration> CreateIncidentRegistrationAsync(Registration registration)
        {
            var created = await db.IncidentRegistrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();
            return (await IncidentRegistrations.SingleAsync(r => r.Id == created.Entity.Id)).ToViewModel();
        }

        public async Task UpdateIncidentRegistrationAsync(Registration registration)
        {
            //var familyMembersToKeep = registration.HeadOfHousehold.FamilyMembers.Where(fm => !string.IsNullOrWhiteSpace(fm.Id)).Select(fm => Guid.Parse(fm.Id)).ToList();
            //var familyMembersToRemove = (await IncidentRegistrations.SingleAsync(x => x.Id == Guid.Parse(registration.Id)))
            //.HeadOfHousehold.FamilyMembers.Where((fm) => !familyMembersToKeep.Contains(fm.Id));

            var familyMembersToKeep = registration.HeadOfHousehold.FamilyMembers.Select(fm => fm.EvacueeSequenceNumber);
            //TODO: Remove Dropped Evacuees of Updated Registrations
            //var familyMembersToRemove = (await Evacuees.)

            db.IncidentRegistrations.Update(registration.ToModel());
            //db.Evacuees.RemoveRange(familyMembersToRemove);

            await db.SaveChangesAsync();
        }
    }
}
