using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.ViewModels;
using HandlebarsDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;

namespace Gov.Jag.Embc.Public.Services.Referrals
{
    public class ReferralsService : IReferralsService
    {
        private readonly IDataInterface dataInterface;
        private readonly IPdfConverter pdfConverter;
        private readonly ICurrentUser userService;
        private readonly IHostingEnvironment env;

        private readonly string pageBreak = $@"{Environment.NewLine}<div class=""page-break""></div>{Environment.NewLine}";

        public ReferralsService(IDataInterface dataInterface, IPdfConverter pdfConverter, ICurrentUser currentUser, IHostingEnvironment environment)
        {
            this.dataInterface = dataInterface;
            this.pdfConverter = pdfConverter;
            this.userService = currentUser;
            this.env = environment;
        }

        public async Task<byte[]> GetReferralPdfsAsync(ReferralsToPrint printReferrals)
        {
            var content = await GetReferralHtmlPagesAsync(printReferrals);

            if (content == null)
            {
                return null;
            }

            return await pdfConverter.ConvertHtmlToPdfAsync(content);
        }

        public async Task<string> GetReferralHtmlPagesAsync(ReferralsToPrint printReferrals)
        {
            var referrals = await dataInterface.GetReferralsAsync(printReferrals.ReferralIds);

            if (!referrals.Any())
            {
                return null;
            }

            var html = AssembleReferralHtml(referrals, printReferrals.AddSummary);

            return html;
        }

        private string AssembleReferralHtml(IEnumerable<PrintReferral> referrals, bool includeSummary)
        {
            var referralHtml = string.Empty;

            foreach (var referral in referrals)
            {
                var newHtml = CreateReferralHtmlPages(referral);
                referralHtml = $"{referralHtml}{newHtml}";
            }

            var summaryHtml = includeSummary ? CreateReferalHtmlSummary(referrals) : string.Empty;
            var finalHtml = $"{summaryHtml}{referralHtml}";

            var handleBars = Handlebars.Create();
            handleBars.RegisterTemplate("stylePartial", GetCSSPartialView());
            handleBars.RegisterTemplate("bodyPartial", finalHtml);
            var template = handleBars.Compile(TemplateLoader.LoadTemplate("MasterLayout"));
            var assembledHtml = template("");

            return assembledHtml;
        }

        private string CreateReferralHtmlPages(PrintReferral referral)
        {
            var handleBars = Handlebars.Create();

            handleBars.RegisterTemplate("stylePartial", GetCSSPartialView());

            var partialViewType = MapToReferralType(referral.Type);

            var partialItemsSource = GetItemsPartialView(partialViewType);
            handleBars.RegisterTemplate("itemsPartial", partialItemsSource);

            handleBars.RegisterTemplate("itemsDetailTitle", string.Empty);

            var partialSupplierSource = GetSupplierPartialView(partialViewType);
            handleBars.RegisterTemplate("supplierPartial", partialSupplierSource);

            var partialChecklistSource = GetChecklistPartialView(partialViewType);
            handleBars.RegisterTemplate("checklistPartial", partialChecklistSource);

            var template = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.Referral.ToString()));

            referral.VolunteerDisplayName = userService.GetDisplayName();
            // If we're in prod, we don't want the watermark
            referral.DisplayWatermark = !env.IsProduction();

            var result = template(referral);

            return $"{result}{pageBreak}";
        }

        private string CreateReferalHtmlSummary(IEnumerable<PrintReferral> referrals)
        {
            var handleBars = Handlebars.Create();

            var result = string.Empty;
            var itemsHtml = string.Empty;
            var summaryBreakCount = 0;
            var printedCount = 0;
            var VolunteerDisplayName = userService.GetDisplayName();
            var purchaserName = referrals.FirstOrDefault()?.Purchaser;
            foreach (var referral in referrals)
            {
                summaryBreakCount += 1;
                printedCount += 1;
                var partialViewType = MapToReferralType(referral.Type);

                handleBars.RegisterTemplate("titlePartial", partialViewType.GetDisplayName());

                var useSummaryVersion = partialViewType == ReferralPartialView.Hotel || partialViewType == ReferralPartialView.Billeting;
                var partialItemsSource = GetItemsPartialView(partialViewType, useSummaryVersion);
                handleBars.RegisterTemplate("itemsPartial", partialItemsSource);

                handleBars.RegisterTemplate("itemsDetailTitle", "Details");

                var partialNotesSource = GetNotesPartialView(partialViewType);
                handleBars.RegisterTemplate("notesPartial", partialNotesSource);

                var template = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.SummaryItem.ToString()));
                var itemResult = template(referral);

                itemsHtml = $"{itemsHtml}{itemResult}";

                if (summaryBreakCount == 3 || printedCount == referrals.Count())
                {
                    summaryBreakCount = 0;

                    handleBars.RegisterTemplate("summaryItemsPartial", itemsHtml);

                    var mainTemplate = handleBars.Compile(TemplateLoader.LoadTemplate(ReferalMainViews.Summary.ToString()));

                    var data = new { VolunteerDisplayName, purchaserName };
                    result = $"{result}{mainTemplate(data)}{pageBreak}";
                    itemsHtml = string.Empty;
                }
            }

            return result;
        }

        private string GetCSSPartialView()
        {
            return TemplateLoader.LoadTemplate("Css");
        }

        private string GetItemsPartialView(ReferralPartialView partialView, bool useSummaryPartial = false)
        {
            var summary = useSummaryPartial ? "Summary" : string.Empty;
            var name = $"{partialView.ToString()}.{partialView.ToString()}Items{summary}Partial";
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

        public bool IsValidReferralType(string type, string subType)
        {
            subType = string.IsNullOrEmpty(subType) ? "" : "_" + subType;
            return Enum.GetNames(typeof(Models.Db.ReferralType)).Any(t => t.Equals($"{type}{subType}", StringComparison.OrdinalIgnoreCase));
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
