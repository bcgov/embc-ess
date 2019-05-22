using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.ViewModels;
using HandlebarsDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            var referralHtml = string.Empty;

            var referrals = await dataInterface.GetReferralsAsync(printReferrals.ReferralIds);

            foreach (var referral in referrals)
            {
                var newHtml = CreateReferralHtmlPages(referral);
                referralHtml = $"{referralHtml}{newHtml}";
            }

            var summaryHtml = printReferrals.AddSummary ? CreateReferalHtmlSummary(referrals) : string.Empty;
            var finalHtml = $"{summaryHtml}{referralHtml}";

            return finalHtml;
        }

        private string CreateReferralHtmlPages(PrintReferral referral)
        {
            var handleBars = Handlebars.Create();

            var partialViewType = MapToReferralType(referral.Type);

            var partialItemsSource = GetItemsPartialView(partialViewType);
            handleBars.RegisterTemplate("itemsPartial", partialItemsSource);

            handleBars.RegisterTemplate("itemsDetailTitle", string.Empty);

            var partialSupplierSource = GetSupplierPartialView(partialViewType);
            handleBars.RegisterTemplate("supplierPartial", partialSupplierSource);

            var partialChecklistSource = GetChecklistPartialView(partialViewType);
            handleBars.RegisterTemplate("checklistPartial", partialChecklistSource);

            var template = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.Referral.ToString()));

            var result = template(referral);

            return result;
        }

        private string CreateReferalHtmlSummary(IEnumerable<PrintReferral> referrals)
        {
            var handleBars = Handlebars.Create();

            var itemsHtml = string.Empty;
            foreach (var referral in referrals)
            {
                var partialViewType = MapToReferralType(referral.Type);

                handleBars.RegisterTemplate("titlePartial", partialViewType.GetDisplayName());

                var partialItemsSource = GetItemsPartialView(partialViewType);
                handleBars.RegisterTemplate("itemsPartial", partialItemsSource);

                handleBars.RegisterTemplate("itemsDetailTitle", "Details");

                var partialNotesSource = GetNotesPartialView(partialViewType);
                handleBars.RegisterTemplate("notesPartial", partialNotesSource);

                var template = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.SummaryItem.ToString()));
                var itemResult = template(referral);

                itemsHtml = $"{itemsHtml}{itemResult}";
            }

            handleBars.RegisterTemplate("summaryItemsPartial", itemsHtml);

            var mainTemplate = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.Summary.ToString()));

            var data = new { purchaser = referrals.First().Purchaser };
            var result = mainTemplate(data);

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

        private string GetNotesPartialView(ReferralPartialView partialView)
        {
            var name = $"{partialView.ToString()}.{partialView.ToString()}NotesPartial";
            return TemplateLoader.LoadTemplate(name);
        }

        public enum ReferalMainViews
        {
            Referral,
            Summary,
            SummaryItem
        }

        public enum ReferralPartialView
        {
            [DisplayName("BILLETING")]
            Billeting,

            [DisplayName("CLOTHING")]
            Clothing,

            [DisplayName("FOOD, GROCERIES")]
            Groceries,

            [DisplayName("GROUP LODGING")]
            GroupLodging,

            [DisplayName("HOTEL/MOTEL")]
            Hotel,

            [DisplayName("INCIDENTALS")]
            Incidentals,

            [DisplayName("FOOD, RESTAURANT MEALS")]
            Meals,

            [DisplayName("TAXI")]
            Taxi,

            [DisplayName("TRANSPORTATION")]
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