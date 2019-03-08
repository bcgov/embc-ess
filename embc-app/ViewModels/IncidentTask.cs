using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{    

    public class IncidentTask
    {
        public string Id { get; set; }
        public string TaskNumber { get; set; }
        public string Details { get; set; }
        public Region Region { get; set; }
        public RegionalDistrict RegionalDistrict { get; set; }
        public Community Community { get; set; }
    }
}
