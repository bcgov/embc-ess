using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public interface IReferralsService
    {
        Task<IActionResult> GetReferralPdfs(ReferralsToPrint printReferrals);

        Task<string> GetReferralHtmlPages(ReferralsToPrint printReferrals);

        bool IsValidReferralType(string type, string subType);
    }
}
