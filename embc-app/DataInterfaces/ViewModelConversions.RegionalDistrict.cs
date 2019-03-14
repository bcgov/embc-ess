using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.RegionalDistrict ToViewModel(this Sqlite.Models.RegionalDistrict source)
        {
            ViewModels.RegionalDistrict result = null;
            if (source != null)
            {
                result = new ViewModels.RegionalDistrict()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active,
                    Region = source.Region.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.RegionalDistrict ToModel(this ViewModels.RegionalDistrict source)
        {
            Sqlite.Models.RegionalDistrict result = null;
            if (source != null)
            {
                result = new Sqlite.Models.RegionalDistrict()
                {
                    Name = source.Name,
                    Active = source.Active,
                    Region = source.Region.ToModel()
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
