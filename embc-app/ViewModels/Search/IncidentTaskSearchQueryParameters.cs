using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.ViewModels.Search
{
    public class IncidentTaskSearchQueryParameters : SearchQueryParameters
    {
        [FromQuery(Name = "activeTasks")]
        public bool? ActiveTasks { get; set; }
    }

    public enum IncidentTaskStatus
    {
        Active,
        Inactive
    }
}
