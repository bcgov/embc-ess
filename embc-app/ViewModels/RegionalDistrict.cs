using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{    

    public class RegionalDistrict
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool? Active { get; set; }

        public Region Region { get; set; }
    }
}
