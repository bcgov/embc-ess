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
        private IQueryable<Models.Db.Referral> Referrals => db.Referrals
                .AsNoTracking()
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

        public async Task<IEnumerable<ViewModels.Referral>> GetReferralsAsync(string registrationId)
        {
            var results = Referrals.Where(r => r.RegistrationId == long.Parse(registrationId));

            return await results.Select(r => mapper.Map<ViewModels.Referral>(r)).ToArrayAsync();
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
