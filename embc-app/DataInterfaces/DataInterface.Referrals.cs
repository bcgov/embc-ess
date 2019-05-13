using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<ViewModels.Referral> CreateReferral(ViewModels.Referral referral)
        {
            var entity = mapper.Map(referral, referral.GetType(), GetReferralType(referral.Type, referral.SubType));

            var created = db.Add(entity);

            await db.SaveChangesAsync();
            return (ViewModels.Referral)mapper.Map(created.Entity, created.Entity.GetType(), typeof(ViewModels.Referral));
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
