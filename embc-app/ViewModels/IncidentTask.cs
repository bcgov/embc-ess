using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class IncidentTask
    {
        public string Id { get; set; }
        public string TaskNumber { get; set; }
        public string Details { get; set; }
        public bool? Active { get; set; }
        public int? TotalAssociatedEvacuees { get; set; }

        // only one of the following will be set; ie a regional incident vs a community one, etc
        public Region Region { get; set; }

        public Community Community { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
