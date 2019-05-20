using Gov.Jag.Embc.Public.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private static List<string> referralOrder = new List<string> { "FOOD", "LODGING", "CLOTHING", "TRANSPORTATION", "INCIDENTALS" };

        private IQueryable<Models.Db.Referral> Referrals => db.Referrals
                .AsNoTracking()
                .Include(r => r.Registration)
                .Include(r => r.Supplier)
                .Include(r => r.Evacuees)
                    .ThenInclude(re => re.Evacuee);

        public async Task<string> CreateReferralAsync(ViewModels.Referral referral)
        {
            var entity = (Models.Db.Referral)mapper.Map(referral, referral.GetType(), GetReferralType(referral.Type, referral.SubType));

            var created = db.Referrals.Add(entity);

            await db.SaveChangesAsync();
            return created.Entity.ReferralId;
        }

        public async Task<ViewModels.Referral> GetReferralAsync(string referralId)
        {
            if (!long.TryParse(referralId.Substring(1), out var id)) throw new ArgumentException($"{referralId} is not valid", nameof(referralId));

            var result = await Referrals.SingleOrDefaultAsync(r => r.Id == id);

            return mapper.Map<ViewModels.Referral>(result);
        }

        public async Task<IPagedResults<ViewModels.ReferralListItem>> GetReferralsAsync(string registrationId, SearchQueryParameters searchQuery)
        {
            var results = await Referrals
                .Where(r => r.RegistrationId == long.Parse(registrationId) && r.Active == searchQuery.Active)
                .Select(r => mapper.Map<ViewModels.Referral>(r))
                .ToArrayAsync();

            return new PaginatedList<ViewModels.ReferralListItem>(
                results.Select(r => r.ToListItem())
                .OrderBy(r => referralOrder.IndexOf(r.Type))
                    .ThenByDescending(r => r.ValidFrom)
                .ToArray(),
                searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<IEnumerable<ViewModels.Referral>> GetReferralsAsync(IEnumerable<string> referralIds)
        {
            var results = await Referrals
                .Where(r => referralIds.Contains(r.ReferralId))
                .Select(r => mapper.Map<ViewModels.Referral>(r))
                .ToArrayAsync();

            return results;
        }

        public async Task<bool> DeactivateReferralAsync(string referralId)
        {
            if (!long.TryParse(referralId.Substring(1), out var id)) throw new ArgumentException($"{referralId} is not valid", nameof(referralId));
            var entity = await db.Referrals.SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }
            entity.Active = false;
            db.Referrals.Update(entity);
            await db.SaveChangesAsync();

            return true;
        }

        private Type GetReferralType(string type, string subType)
        {
            var typeName = $"{subType}{type}Referral";
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => typeof(Models.Db.Referral).IsAssignableFrom(t) && t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
