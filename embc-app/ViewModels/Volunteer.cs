namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Volunteer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }  
        public string BceidAccountNumber { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }
        // siteminder guid
        public string Externaluseridentifier { get; set; }

        // related entities
        public Organization Organization { get; set; }
    }
}
