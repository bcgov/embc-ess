namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Volunteer : Person
    {
        public string Name { get; set; } // TODO: What's this? Their display name? full name?
        public string Email { get; set; }
        public string BceidAccountNumber { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        // related entities
        public Organization Organization { get; set; }

        // siteminder guid
        public string Externaluseridentifier { get; set; }

        public Volunteer()
        {
            PersonType = Sqlite.Models.Person.VOLUNTEER;
        }
    }
}
