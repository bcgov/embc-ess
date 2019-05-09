using Gov.Jag.Embc.Public.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public abstract class Person
    {
        public static Person Create(string subType)
        {
            if (subType == Models.Db.Person.HOH)
            {
                return new HeadOfHousehold();
            }
            if (subType == Models.Db.Person.FAMILY_MEMBER)
            {
                return new FamilyMember();
            }
            return null;
        }

        public string Id { get; set; }
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.
        public string PersonType { get; set; }  // one of 'VOLN' (volunteer), 'HOH' (head of household), 'FMBR' (family member)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Initials { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? Dob { get; set; }
    }
}
