using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public abstract class Person
    {
        public static Person Create(string subType)
        {
            if (subType == Sqlite.Models.Person.VOLUNTEER)
            {
                return new Volunteer();
            }
            else if (subType == Sqlite.Models.Person.HOH)
            {
                return new HeadOfHousehold();
            }
            else if (subType == Sqlite.Models.Person.FAMILY_MEMBER)
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
        public DateTimeOffset? Dob { get; set; }
    }
}
