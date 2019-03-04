using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{

    

    public class FamilyMember: Person
    {
        public string RelationshipToEvacuee { get; set; }

        public bool SameLastNameAsEvacuee { get; set; }

    }
}
