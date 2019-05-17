using Gov.Jag.Embc.Public.DataInterfaces;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public class ReferralsService : IReferralsService
    {
        private readonly IDataInterface dataInterface;

        public ReferralsService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        //Restaurant Meals
        //GROCERIES
        //Hotel/Motel
        //Group Lodging
        //Billeting
        //Clothing
        //Taxi
        //Incidentals

        public async Task<string> GetReferralHtmlPages(IEnumerable<string> referralIds)
        {
            var html = string.Empty;

            var referrals = await dataInterface.GetReferralsAsync(referralIds);

            foreach (var referral in referrals)
            {
                var newHtml = CreateReferralHtmlContent(referral);
                html = $"{html}{newHtml}";
            }

            return html;
        }

        private string CreateReferralHtmlContent(ViewModels.Referral referral)
        {
            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var template = handleBars.CompileView("Services/Referrals/Views/layout.hbs");
            var result = template(referral);

            return result;
        }
    }
}
