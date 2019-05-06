using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
    }

    public class Referral
    {
        public string Purchaser { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public IEnumerable<string> Evacuees { get; set; }
        public Supplier Supplier { get; set; }
        public string Comments { get; set; }
        public decimal TotalAmount { get; set; }
        public bool ConfirmChecked { get; set; }
    }
}
