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
        public async Task<Registration> CreateRegistrationAsync(Registration registration)
        {
            var created = await db.Registrations.AddAsync(registration.ToModel());
            await db.SaveChangesAsync();

            //created.Entity.EssFileNumber = Math.Abs(created.Entity.Id.GetHashCode()); //TODO: replace with DB based sequence
            await db.SaveChangesAsync();

            return await GetRegistrationAsync(created.Entity.Id.ToString());
        }

        public async Task UpdateRegistrationAsync(Registration registration)
        {
            db.Registrations.Update(registration.ToModel());
            await db.SaveChangesAsync();
        }

        public async Task<PaginatedList<Registration>> GetRegistrationsAsync(SearchQueryParameters queryParameters)
        {
            var q = queryParameters.Query;

            IQueryable<Models.Db.Registration> registrations = db.Registrations
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
                )
                ;

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
            if (Guid.TryParse(id, out var guid))
            {
                var entity = await db.Registrations.FirstOrDefaultAsync(reg => reg.Id == guid);
                return entity?.ToViewModel();
            }
            return null;
        }

        public async Task<RegistrationSummary> GetRegistrationSummaryAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                var entity = await db.Registrations.FirstOrDefaultAsync(reg => reg.Id == guid);
                return entity?.ToSummaryViewModel();
            }
            return null;
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
