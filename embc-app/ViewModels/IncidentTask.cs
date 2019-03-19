namespace Gov.Jag.Embc.Public.ViewModels
{
    public class IncidentTask
    {
        public string Id { get; set; }
        public string TaskNumber { get; set; }
        public string Details { get; set; }
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.

        // only one of the following will be set; ie a regional incident vs a community one, etc
        public Region Region { get; set; }
        public RegionalDistrict RegionalDistrict { get; set; }
        public Community Community { get; set; }
    }
}
