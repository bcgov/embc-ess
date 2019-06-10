using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.ViewModels.Search
{
    public class EvacueeSearchQueryParameters : SearchQueryParameters
    {
        [FromQuery(Name = "last_name")]
        public string LastName { get; set; }

        [FromQuery(Name = "last_name")]
        public string FirstName { get; set; }

        [FromQuery(Name = "task_no")]
        public string IncidentTaskId { get; set; }

        [FromQuery(Name = "ess_file_no")]
        public string EssFileNumber { get; set; }

        [FromQuery(Name = "evacuated_from")]
        public string EvacuatedFrom { get; set; }

        [FromQuery(Name = "evacuated_to")]
        public string EvacuatedTo { get; set; }

        [FromQuery(Name = "referrals_provided")]
        public bool? HasReferrals { get; set; }

        [FromQuery(Name = "registration_completed")]
        public bool? RegistrationCompleted { get; set; }
    }
}
