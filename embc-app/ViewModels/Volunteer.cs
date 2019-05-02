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
        public string BceidAccountUserName { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }
        public string Externaluseridentifier { get; set; }
        public string SiteMinderGuid { get; set; }
        public Organization Organization { get; set; }
    }
}
