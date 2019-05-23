using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public interface IReferralsService
    {
        Task<byte[]> GetReferralPdfsAsync(ReferralsToPrint printReferrals);

        bool IsValidReferralType(string type, string subType);
    }
}