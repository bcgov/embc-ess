using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public interface IReferralsService
    {
        Task<string> GetReferralHtmlPages(IEnumerable<string> referralIds);
    }
}
