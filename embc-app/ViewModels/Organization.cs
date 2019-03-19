namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Organization
    {
        public string Id { get; set; } // Guid
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.
        public string Name { get; set; }
        public string BceidAccountNumber { get; set; }
        public string Externaluseridentifier { get; set; }
        //public Person PrimaryContact { get; set; }
    }
}
