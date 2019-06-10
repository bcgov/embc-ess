using Gov.Jag.Embc.Public.Utils;

namespace Gov.Jag.Embc.Public.ViewModels.Search
{
    public class EvacueesSearchQueryParameters : SearchQueryParameters
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string IncidentTaskId { get; set; }
        public string EssFileNumber { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public bool? HasReferrals { get; set; }
        public bool? RegistrationCompleted { get; set; }
    }
}
