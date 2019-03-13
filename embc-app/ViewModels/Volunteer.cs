namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Volunteer : Person
    {
        public string BceidAccountNumber { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }

        // related entities
        public Organization Organization { get; set; }

        public Volunteer()
        {
            PersonType = "VOLN";
        }
    }
}
