using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.IncidentTask ToViewModel(this Sqlite.Models.IncidentTask source)
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
                    RegionalDistrict = source.RegionalDistrict?.ToViewModel(),
                    Community = source.Community?.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.IncidentTask ToModel(this ViewModels.IncidentTask source)
        {
            Sqlite.Models.IncidentTask result = null;
            if (source != null)
            {
                result = new Sqlite.Models.IncidentTask()
                {
                    TaskNumber = source.TaskNumber,
                    Details = source.Details,
                    Active = source.Active,
                    Region = source.Region?.ToModel(),
                    RegionalDistrict = source.RegionalDistrict?.ToModel(),
                    Community = source.Community?.ToModel()
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
            }
            return result;
        }
    }
}
