using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class PrintReferralMappingProfile : Profile
    {
        public PrintReferralMappingProfile()
        {
            CreateMap<Models.Db.Referral, PrintReferral>()
                .ForMember(d => d.IncidentTaskNumber, m => m.MapFrom(s => s.Registration.IncidentTask.TaskNumber))
                .ForMember(d => d.HostCommunity, m => m.MapFrom(s => s.Registration.HostCommunity.Name))
                .ForMember(d => d.PrintEvacuees, m => m.MapFrom(s => s.Evacuees.Select(e => new PrintEvacuee
                {
                    Id = e.Evacuee.EvacueeSequenceNumber.ToString(),
                    FirstName = e.Evacuee.FirstName,
                    LastName = e.Evacuee.LastName,
                    EvacueeTypeCode = MapPrintEvacueeTypeCode(e.Evacuee)
                })))
                .IncludeBase<Models.Db.Referral, Referral>();

            CreateMap<Models.Db.ClothingReferral, PrintReferral>()
                .IncludeBase<Models.Db.Referral, PrintReferral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Clothing));

            CreateMap<Models.Db.IncidentalsReferral, PrintReferral>()
                .IncludeBase<Models.Db.Referral, PrintReferral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Incidentals));

            CreateMap<Models.Db.TransportationReferral, PrintReferral>()
                .IncludeBase<Models.Db.Referral, PrintReferral>();

            CreateMap<Models.Db.TaxiTransportationReferral, PrintReferral>()
                .IncludeBase<Models.Db.TransportationReferral, PrintReferral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Transportation_Taxi));

            CreateMap<Models.Db.OtherTransportationReferral, PrintReferral>()
                .IncludeBase<Models.Db.TransportationReferral, PrintReferral>()
                .ForMember(d => d.OtherTransportModeDetails, m => m.MapFrom(s => s.TransportMode))
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Transportation_Other));

            CreateMap<Models.Db.FoodReferral, PrintReferral>()
                .IncludeBase<Models.Db.Referral, PrintReferral>();

            CreateMap<Models.Db.GroceriesFoodReferral, PrintReferral>()
                .IncludeBase<Models.Db.FoodReferral, PrintReferral>()
                .ForMember(d => d.IncidentTaskNumber, m => m.MapFrom(s => s.Registration.IncidentTask.TaskNumber))
                .ForMember(d => d.NumDaysMeals, m => m.MapFrom(s => s.NumberOfMeals))
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Food_Groceries));

            CreateMap<Models.Db.RestaurantFoodReferral, PrintReferral>()
                .IncludeBase<Models.Db.FoodReferral, PrintReferral>()
                .ForMember(d => d.NumBreakfasts, m => m.MapFrom(s => s.NumberOfBreakfasts))
                .ForMember(d => d.NumLunches, m => m.MapFrom(s => s.NumberOfLunches))
                .ForMember(d => d.NumDinners, m => m.MapFrom(s => s.NumberOfDinners))
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Food_Restaurant));

            CreateMap<Models.Db.LodgingReferral, PrintReferral>()
                .IncludeBase<Models.Db.Referral, PrintReferral>()
                .ForMember(d => d.NumNights, m => m.MapFrom(s => s.NumberOfNights));

            CreateMap<Models.Db.HotelLodgingReferral, PrintReferral>()
               .IncludeBase<Models.Db.LodgingReferral, PrintReferral>()
               .ForMember(d => d.NumRooms, m => m.MapFrom(s => s.NumberOfRooms))
               .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Hotel));

            CreateMap<Models.Db.GroupLodgingReferral, PrintReferral>()
                .IncludeBase<Models.Db.LodgingReferral, PrintReferral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Group));

            CreateMap<Models.Db.BilletingLodgingReferral, PrintReferral>()
                .IncludeBase<Models.Db.LodgingReferral, PrintReferral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Billeting));
        }

        private string MapPrintEvacueeTypeCode(Models.Db.Evacuee evacuee)
        {
            if (evacuee.EvacueeType == Models.Db.Enumerations.EvacueeType.HeadOfHousehold)
            {
                return "F";
            }
            if (evacuee.Dob.HasValue && evacuee.Dob.Value > DateTime.Now.AddYears(-12))
            {
                return "C";
            }
            return "A";
        }
    }

    public class PrintReferral : Referral
    {
        public string IncidentTaskNumber { get; set; }
        public string HostCommunity { get; set; }
        public string FromDate => ValidDates.From.ToString("MMM-dd-yyyy");
        public string FromTime => ValidDates.From.ToString("h:mm tt");
        public string ToDate => ValidDates.To.ToString("MMM-dd-yyyy");
        public string ToTime => ValidDates.To.ToString("h:mm tt");
        public string PrintDate => DateTime.Today.ToString("MMM-dd-yyyy");
        public IEnumerable<PrintEvacuee> PrintEvacuees { get; set; }
        public string TotalAmountPrinted => TotalAmount.ToString("N2");
        public string CommentsPrinted => ConvertCarriageReturnToHtml(Comments);
        public string ApprovedItemsPrinted => ConvertCarriageReturnToHtml(ApprovedItems);

        public object[] PrintableEvacuees
        {
            get
            {
                var evacueesToPrint = new List<object>();
                var evacuees = PrintEvacuees.ToArray();

                for (int i = 0; i <= 7; i++)
                {
                    evacueesToPrint.Add(new PrintableEvacueesRow(evacuees.ElementAtOrDefault(i), evacuees.ElementAtOrDefault(i + 7)));
                }
                return evacueesToPrint.ToArray();
            }
        }

        private string ConvertCarriageReturnToHtml(string value)
        {
            return value?.Replace("\n", "<br />")?.Replace("\r", "<br />");
        }
    }

    public class PrintEvacuee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EvacueeTypeCode { get; set; }
    }

    public class PrintableEvacueesRow
    {
        public PrintableEvacueesRow(PrintEvacuee referralEvacuee1, PrintEvacuee referralEvacuee2)
        {
            Column1 = GetEvacueeColumn(referralEvacuee1);
            Column2 = GetEvacueeColumn(referralEvacuee2);
        }

        public string Column1 { get; private set; }
        public string Column1Class => GetEvacueeColumnClass(Column1);
        public string Column2 { get; private set; }
        public string Column2Class => GetEvacueeColumnClass(Column2);

        private string GetEvacueeColumn(PrintEvacuee referralEvacuee)
        {
            if (referralEvacuee == null)
            {
                return string.Empty;
            }

            return $"{referralEvacuee.LastName}, {referralEvacuee.FirstName} ({referralEvacuee.EvacueeTypeCode})";
        }

        private string GetEvacueeColumnClass(string columnText)
        {
            return string.IsNullOrEmpty(columnText) ? "nobody" : "evacuee";
        }
    }
}
