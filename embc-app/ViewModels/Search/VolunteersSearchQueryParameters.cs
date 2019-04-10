using Gov.Jag.Embc.Public.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Gov.Jag.Embc.Public.ViewModels.Search
{
    public class VolunteersSearchQueryParameters : SearchQueryParameters
    {
        [FromQuery(Name = "ess_only")]
        public bool? OnlyEssUsers { get; set; }

        [FromQuery(Name = "admin_only")]
        public bool? OnlyAdminUsers { get; set; }

        [FromQuery(Name = "org_id")]
        public string OrganizationId { get; set; }
    }
}
