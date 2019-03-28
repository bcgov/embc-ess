using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.ViewModels.Search
{
    public class VolunteersSearchQueryParameters : SearchQueryParameters
    {
        [FromQuery(Name = "ess")]
        public bool? OnlyEssUsers { get; set; }

        [FromQuery(Name = "admin")]
        public bool? OnlyAdminUsers { get; set; }

        [FromQuery(Name = "notactive")]
        public bool? IncludeDeactivated { get; set; }

        [FromQuery(Name = "orgid")]
        public string OrganizationId { get; set; }
    }
}
