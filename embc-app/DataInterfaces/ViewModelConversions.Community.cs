using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Community ToViewModel(this Sqlite.Models.Community source)
        {
            ViewModels.Community result = null;
            if (source != null)
            {
                result = new ViewModels.Community()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active,
                    RegionalDistrict = source.RegionalDistrict.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.Community ToModel(this ViewModels.Community source)
        {
            Sqlite.Models.Community result = null;
            if (source != null)
            {
                result = new Sqlite.Models.Community()
                {
                    Name = source.Name,
                    Active = source.Active,
                    RegionalDistrict = source.RegionalDistrict.ToModel()
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
