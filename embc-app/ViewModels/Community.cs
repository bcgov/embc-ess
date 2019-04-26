namespace Gov.Jag.Embc.Public.ViewModels
{

    public class Community
    {
        public string Id { get; set; }
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.
        public string Name { get; set; }

        // related entities
        public Region Region { get; set; }
    }
}
