using Gov.Jag.Embc.Public.ViewModels;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public interface IReferralsService
    {
        Task<string> GetReferralHtmlPages(ReferralsToPrint printReferrals);
    }
}
