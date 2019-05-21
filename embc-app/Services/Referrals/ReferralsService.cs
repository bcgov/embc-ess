using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.ViewModels;
using HandlebarsDotNet;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.Utils;
using System;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public class ReferralsService : IReferralsService
    {
        private readonly IDataInterface dataInterface;

        public ReferralsService(IDataInterface dataInterface)
        {
            this.dataInterface = dataInterface;
        }

        public async Task<string> GetReferralHtmlPages(ReferralsToPrint printReferrals)
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

        private string CreateReferralHtmlPages(PrintReferral referral)
        {
            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var partialViewType = MapToReferralType(referral.Type);

            var partialItemsSource = GetItemsPartialView(partialViewType);
            handleBars.RegisterTemplate("itemsPartial", partialItemsSource);

            var partialSupplierSource = GetSupplierPartialView(partialViewType);
            handleBars.RegisterTemplate("supplierPartial", partialSupplierSource);

            var partialChecklistSource = GetChecklistPartialView(partialViewType);
            handleBars.RegisterTemplate("checklistPartial", partialChecklistSource);

            var template = handleBars.Compile(TemplateLoader.LoadTemplate(ReferallMainViews.Master.ToString()));

            var result = template(referral);

            return result;
        }

        private string CreateReferalHtmlSummary()
        {
            var data = new { test = "" };
            var handleBars = Handlebars.Create(new HandlebarsConfiguration { FileSystem = new DiskFileSystem() });

            var template = handleBars.CompileView("Services/Referrals/Views/Summary.hbs");
            var result = template(data);

            return result;
        }

        private string GetItemsPartialView(ReferralPartialView partialView)
        {
            var name = $"{partialView.ToString()}.{partialView.ToString()}ItemsPartial";
            return TemplateLoader.LoadTemplate(name);
        }

        private string GetChecklistPartialView(ReferralPartialView partialView)
        {
            var name = $"{partialView.ToString()}.{partialView.ToString()}ChecklistPartial";
            return TemplateLoader.LoadTemplate(name);
        }

        private string GetSupplierPartialView(ReferralPartialView partialView)
        {
            var name = $"{partialView.ToString()}.{partialView.ToString()}SupplierPartial";
            return TemplateLoader.LoadTemplate(name);
        }

        public enum ReferallMainViews
        {
            Master,
            Summary
        }

        public enum ReferralPartialView
        {
            Billeting,
            Clothing,
            Groceries,
            GroupLodging,
            Hotel,
            Incidentals,
            Meals,
            Taxi,
            Transportation
        }

        private ReferralPartialView MapToReferralType(string referralType)
        {
            var referralTypeEnum = EnumHelper<ReferralType>.GetValueFromName(referralType);
            switch (EnumHelper<ReferralType>.GetValueFromName(referralType))
            {
                case ReferralType.Clothing:
                    return ReferralPartialView.Clothing;

                case ReferralType.Food_Groceries:
                    return ReferralPartialView.Groceries;

                case ReferralType.Food_Restaurant:
                    return ReferralPartialView.Meals;

                case ReferralType.Incidentals:
                    return ReferralPartialView.Incidentals;

                case ReferralType.Lodging_Billeting:
                    return ReferralPartialView.Billeting;

                case ReferralType.Lodging_Group:
                    return ReferralPartialView.GroupLodging;

                case ReferralType.Lodging_Hotel:
                    return ReferralPartialView.Hotel;

                case ReferralType.Transportation_Other:
                    return ReferralPartialView.Transportation;

                case ReferralType.Transportation_Taxi:
                    return ReferralPartialView.Taxi;

                default:
                    throw new ArgumentException($"{referralType} not a valid ReferralType");
            }
        }
    }
}
