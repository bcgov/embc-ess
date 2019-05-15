using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class PrintReferrals
    {
        public IEnumerable<string> ReferralIds { get; set; }
        public bool AddSummary { get; set; }
    }
}
