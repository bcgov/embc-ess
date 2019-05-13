using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public abstract class Referral : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public bool Active { get; set; }

        [NotMapped]
        public string ReferralId => $"D{Id}";

        public long RegistrationId { get; set; }

        public EvacueeRegistration Registration { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidTo { get; set; }
        public IEnumerable<ReferralEvacuee> Evacuees { get; set; }

        [Required]
        public Supplier Supplier { get; set; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal TotalAmount { get; set; }

        public bool ConfirmChecked { get; set; }
        public string Comments { get; set; }

        [MaxLength(255)]
        public string Purchaser { get; set; }
    }

    [Table("ReferralEvacuees")]
    public class ReferralEvacuee
    {
        public long ReferralId { get; set; }
        public Referral Referral { get; set; }
        public long RegistrationId { get; set; }
        public int EvacueeId { get; set; }
        public Evacuee Evacuee { get; set; }
    }

    public enum ReferralType
    {
        Food_Groceries,
        Food_Restaurant,
        Transportation_Taxi,
        Transportation_Other,
        Lodging_Hotel,
        Lodging_Group,
        Lodging_Billeting,
        Incidentals,
        Clothing
    }

    public abstract class FoodReferral : Referral
    {
    }

    public class GroceriesFoodReferral : FoodReferral
    {
        public int NumberOfMeals { get; set; }
    }

    public class RestaurantFoodReferral : FoodReferral
    {
        public int NumberOfBreakfasts { get; set; }
        public int NumberOfLunches { get; set; }
        public int NumberOfDinners { get; set; }
    }

    public abstract class TransportationReferral : Referral
    {
    }

    public class TaxiTransportationReferral : TransportationReferral
    {
        [MaxLength(255)]
        public string FromAddress { get; set; }

        [MaxLength(255)]
        public string ToAddress { get; set; }
    }

    public class OtherTransportationReferral : TransportationReferral
    {
        [MaxLength(255)]
        public string TransportMode { get; set; }
    }

    public abstract class LodgingReferral : Referral
    {
        public int NumberOfNights { get; set; }
    }

    public class HotelLodgingReferral : LodgingReferral
    {
        public int NumberOfRooms { get; set; }
    }

    public class BilletingLodgingReferral : LodgingReferral
    {
    }

    public class GroupLodgingReferral : LodgingReferral
    {
    }

    public class ClothingReferral : Referral
    {
        public bool ExtremeWinterConditions { get; set; }
    }

    public class IncidentalsReferral : Referral
    {
        public string ApprovedItems { get; set; }
    }
}
