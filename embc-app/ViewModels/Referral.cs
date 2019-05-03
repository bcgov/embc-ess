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
    }
}
