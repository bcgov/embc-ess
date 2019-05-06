using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.IncidentTask ToViewModel(this Models.Db.IncidentTask source)
        {
            ViewModels.IncidentTask result = null;
            if (source != null)
            {
                result = new ViewModels.IncidentTask()
                {
                    Id = source.Id.ToString(),
                    TaskNumber = source.TaskNumber,
                    Details = source.Details,
                    Active = source.Active,
                    Region = source.Region?.ToViewModel(),
                    Community = source.Community?.ToViewModel(),
                    TotalAssociatedEvacuees = source.EvacueeRegistrations?.Count(),
                    StartDate = source.StartDate?.DateTime
                };
            }
            return result;
        }

        public static Models.Db.IncidentTask ToModel(this ViewModels.IncidentTask source)
        {
            Models.Db.IncidentTask result = null;
            if (source != null)
            {
                result = new Models.Db.IncidentTask
                {
                    TaskNumber = source.TaskNumber,
                    Details = source.Details,
                    RegionName = source.Region?.Name,
                    CommunityId = source.Community != null ? Guid.Parse(source.Community.Id) : (Guid?)null,
                    StartDate = source.StartDate
                };

                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                if (source.Active.HasValue)
                {
                    result.Active = source.Active.Value;
                }
            }
            return result;
        }
    }
}
