using System;

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
                    RegionalDistrict = source.RegionalDistrict?.ToViewModel(),
                    Community = source.Community?.ToViewModel()
                };
            }
            return result;
        }

        public static Models.Db.IncidentTask ToModel(this ViewModels.IncidentTask source)
        {
            Models.Db.IncidentTask result = null;
            if (source != null)
            {
                result = new Models.Db.IncidentTask();
                result.PatchValues(source);
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
            }
            return result;
        }
    }
}
