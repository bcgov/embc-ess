using Gov.Jag.Embc.Public.ViewModels;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public interface IReferralsService
    {
        Task<byte[]> GetReferralPdfsAsync(ReferralsToPrint printReferrals);

        Task<string> GetReferralHtmlPagesAsync(ReferralsToPrint printReferrals);

        bool IsValidReferralType(string type, string subType);
    }
}
