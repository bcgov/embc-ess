using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.ViewModels;
using HandlebarsDotNet;
using System;
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

        public async Task<string> GetReferralHtmlPages(PrintReferrals printReferrals)
        {
            var html = string.Empty;

            var referrals = await dataInterface.GetReferralsAsync(printReferrals.ReferralIds);

            foreach (var referral in referrals)
            {
                var newHtml = CreateReferralHtmlPages(referral);
                html = $"{html}{newHtml}";
            }

            html = printReferrals.AddSummary ? $"{CreateReferalHtmlSummary()}{html}" : html;

            return html;
        }

        private string CreateReferralHtmlPages(Referral referral)
        {
            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var template = handleBars.CompileView("Services/Referrals/Views/layout.hbs");
            var result = template(referral);

            return result;
        }

        private string CreateReferalHtmlSummary()
        {
            var data = new { test = "" };
            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var template = handleBars.CompileView("Services/Referrals/Views/summary.hbs");
            var result = template(data);

            return result;
        }

        public bool IsValidReferralType(string type, string subType)
        {
            subType = string.IsNullOrEmpty(subType) ? "" : "_" + subType;
            return Enum.IsDefined(typeof(Models.Db.ReferralType), $"{type}{subType}");
        }
    }
}