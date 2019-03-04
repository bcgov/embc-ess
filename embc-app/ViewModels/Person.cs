using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{

    

    public class Person
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Nickname { get; set; }
        public string Initials { get; set; }
        public string Gender { get; set; }
        public long Dob { get; set; }

        public bool? IsEvacuee { get; set; }
        public bool? IsVolunteer { get; set; }
        public bool? IsFamilyMember { get; set; }

    }
}
