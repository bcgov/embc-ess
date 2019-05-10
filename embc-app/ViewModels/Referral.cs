using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class ReferralMappingProfile : Profile
    {
        public ReferralMappingProfile()
        {
            CreateMap<Referral, Models.Db.Referral>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.ReferralId))
                .ForMember(d => d.Registration, m => m.Ignore())
                .ForMember(d => d.RegistrationId, m => m.MapFrom(s => long.Parse(s.RegistrationId)))
                .ForMember(d => d.TotalAmount, m => m.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.ValidFrom, m => m.MapFrom(s => s.ValidFrom))
                .ForMember(d => d.ValidTo, m => m.MapFrom(s => s.ValidTo))
                .ForMember(d => d.Supplier, m => m.MapFrom(s => s.Supplier))
                .ForMember(d => d.Comments, m => m.MapFrom(s => s.Comments))
                .ForMember(d => d.ConfirmChecked, m => m.MapFrom(s => s.ConfirmChecked))
                .ForMember(d => d.Active, m => m.MapFrom(s => s.Active))
                .ForMember(d => d.Evacuees, m => m.MapFrom(s => s.Evacuees.Select(e => new Models.Db.ReferralEvacuee
                {
                    EvacueeId = int.Parse(e.Id),
                    IsPurchaser = false,
                    RegistrationId = long.Parse(s.RegistrationId)
                }).Append(new Models.Db.ReferralEvacuee
                {
                    EvacueeId = int.Parse(s.Purchaser),
                    IsPurchaser = true,
                    RegistrationId = long.Parse(s.RegistrationId)
                })))
                .ReverseMap()
                .ForMember(d => d.ReferralId, m => m.MapFrom(s => s.ReferralId))
                .ForMember(d => d.RegistrationId, m => m.MapFrom(s => s.RegistrationId.ToString()))
                .ForMember(d => d.TotalAmount, m => m.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.ValidFrom, m => m.MapFrom(s => s.ValidFrom))
                .ForMember(d => d.ValidTo, m => m.MapFrom(s => s.ValidTo))
                .ForMember(d => d.Supplier, m => m.MapFrom(s => s.Supplier))
                .ForMember(d => d.Comments, m => m.MapFrom(s => s.Comments))
                .ForMember(d => d.ConfirmChecked, m => m.MapFrom(s => s.ConfirmChecked))
                .ForMember(d => d.Active, m => m.MapFrom(s => s.Active))
                .ForMember(d => d.Purchaser, m => m.MapFrom(s => s.Evacuees.Single(e => e.IsPurchaser).EvacueeId))
                .ForMember(d => d.Evacuees, m => m.MapFrom(s => s.Evacuees.Where(e => !e.IsPurchaser).Select(e => new ReferralEvacuee
                {
                    Id = e.EvacueeId.ToString(),
                })))
            ;

            CreateMap<Supplier, Models.Db.Supplier>();

            CreateMap<Referral, Models.Db.ClothingReferral>()
                .IncludeBase<Referral, Models.Db.Referral>()
                .ForMember(d => d.ExtremeWeatherConditions, m => m.MapFrom(s => s.ExtremeWinterConditions))
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Clothing.ToString()));

            CreateMap<Referral, Models.Db.IncidentalsReferral>()
                .IncludeBase<Referral, Models.Db.Referral>()
                .ForMember(d => d.ApprovedItems, m => m.MapFrom(s => s.ApprovedItems))
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Incidentals.ToString()));

            CreateMap<Referral, Models.Db.TransportationReferral>()
                .IncludeBase<Referral, Models.Db.Referral>()
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Transportation_Other.ToString().Split('_')[0]));

            CreateMap<Referral, Models.Db.TaxiTransportationReferral>()
                .IncludeBase<Referral, Models.Db.TransportationReferral>()
                .ForMember(d => d.FromAddress, m => m.MapFrom(s => s.FromAddress))
                .ForMember(d => d.ToAddress, m => m.MapFrom(s => s.ToAddress))
                .ReverseMap()
                .IncludeBase<Models.Db.TransportationReferral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Transportation_Taxi.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.OtherTransportationReferral>()
                .IncludeBase<Referral, Models.Db.TransportationReferral>()
                .ForMember(d => d.TransportMode, m => m.MapFrom(s => s.OtherTransportModeDetails))
                .ReverseMap()
                .IncludeBase<Models.Db.TransportationReferral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Transportation_Other.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.FoodReferral>()
                .IncludeBase<Referral, Models.Db.Referral>()
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Food_Groceries.ToString().Split('_')[0]));

            CreateMap<Referral, Models.Db.GroceriesFoodReferral>()
                .IncludeBase<Referral, Models.Db.FoodReferral>()
                .ForMember(d => d.NumberOfMeals, m => m.MapFrom(s => s.NumDaysMeals))
                .ReverseMap()
                .IncludeBase<Models.Db.FoodReferral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Food_Groceries.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.RestaurantFoodReferral>()
                .IncludeBase<Referral, Models.Db.FoodReferral>()
                .ForMember(d => d.NumberOfBreakfasts, m => m.MapFrom(s => s.NumBreakfasts))
                .ForMember(d => d.NumberOfLunches, m => m.MapFrom(s => s.NumLunches))
                .ForMember(d => d.NumberOfDinners, m => m.MapFrom(s => s.NumDinners))
                .ReverseMap()
                .IncludeBase<Models.Db.FoodReferral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Food_Restaurant.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.LodgingReferral>()
                .IncludeBase<Referral, Models.Db.Referral>()
                .ForMember(d => d.NumberOfNights, m => m.MapFrom(s => s.NumNights))
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.Type, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Hotel.ToString().Split('_')[0]));

            CreateMap<Referral, Models.Db.HotelLodgingReferral>()
                .IncludeBase<Referral, Models.Db.LodgingReferral>()
                .ForMember(d => d.NumberOfRooms, m => m.MapFrom(s => s.NumRooms))
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Hotel.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.GroupLodgingReferral>()
                .IncludeBase<Referral, Models.Db.LodgingReferral>()
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Group.ToString().Split('_')[1]));

            CreateMap<Referral, Models.Db.BilletingLodgingReferral>()
                .IncludeBase<Referral, Models.Db.LodgingReferral>()
                .ReverseMap()
                .IncludeBase<Models.Db.Referral, Referral>()
                .ForMember(d => d.SubType, m => m.MapFrom((s, _) => Models.Db.ReferralType.Lodging_Billeting.ToString().Split('_')[1]));
        }
    }

    public class ReferralList
    {
        public string RegistrationId { get; set; }
        public IEnumerable<ReferralListItem> Referrals { get; set; }
    }

    public class ReferralListItem
    {
        public string ReferralId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public bool Active { get; set; }
    }

    public class Supplier
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
    }

    public class Referral
    {
        public string ReferralId { get; set; }
        public string RegistrationId { get; set; }
        public bool Active { get; set; }

        [Required]
        public string Purchaser { get; set; }

        [Required]
        public string Type { get; set; }

        public string SubType { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public IEnumerable<ReferralEvacuee> Evacuees { get; set; }
        public Supplier Supplier { get; set; }
        public string Comments { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public bool ConfirmChecked { get; set; }

        public int? NumBreakfasts { get; set; }
        public int? NumLunches { get; set; }
        public int? NumDinners { get; set; }
        public int? NumDaysMeals { get; set; }
        public int? NumNights { get; set; }
        public int? NumRooms { get; set; }
        public string ApprovedItems { get; set; }
        public bool ExtremeWinterConditions { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string OtherTransportModeDetails { get; set; }

        public ReferralListItem ToListItem()
        {
            return new ReferralListItem
            {
                Active = Active,
                ReferralId = ReferralId,
                SubType = SubType,
                Type = Type,
                Supplier = Supplier,
                ValidFrom = ValidFrom,
                ValidTo = ValidTo
            };
        }
    }

    public class ReferralEvacuee
    {
        public string Id { get; set; }
    }
}
