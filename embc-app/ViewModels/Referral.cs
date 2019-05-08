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

        [Required]
        public string Purchaser { get; set; }

        [Required]
        public string Type { get; set; }

        public string SubType { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public IEnumerable<string> Evacuees { get; set; }
        public Supplier Supplier { get; set; }
        public string Comments { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public bool ConfirmChecked { get; set; }
    }
}
