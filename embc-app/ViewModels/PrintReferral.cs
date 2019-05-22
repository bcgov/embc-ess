using AutoMapper;
using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class PrintReferralMappingProfile : Profile
    {
        public PrintReferralMappingProfile()
        {
            CreateMap<Models.Db.Referral, PrintReferral>()
                .ForMember(d => d.IncidentTaskNumber, m => m.MapFrom(s => s.Registration.IncidentTask.TaskNumber))
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
    }

    public class PrintReferral : Referral
    {
        public string IncidentTaskNumber { get; set; }
        public string FromDate => ValidDates.From.ToString("MMMM-dd-yyyy");
        public string FromTime => ValidDates.From.ToString("h:mm tt");
        public string ToDate => ValidDates.To.ToString("MMMM-dd-yyyy");
        public string ToTime => ValidDates.To.ToString("h:mm tt");
        public string PrintDate => DateTime.Today.ToString("MMMM-dd-yyyy");
    }
}
