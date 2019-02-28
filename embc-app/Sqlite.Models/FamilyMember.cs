using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public sealed partial class FamilyMember : Person
    {

        public FamilyMember()
        { }
        
        [MaxLength(255)]        
        public string RelationshipToEvacuee { get; set; }

        public bool SameLastNameAsEvacuee { get; set; }

    }
}
