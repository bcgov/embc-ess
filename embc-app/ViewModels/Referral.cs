using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
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

        public IEnumerable<Evacuee> Evacuees { get; set; }
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
}
