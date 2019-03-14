using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Region ToViewModel(this Sqlite.Models.Region source)
        {
            ViewModels.Region result = null;
            if (source != null)
            {
                result = new ViewModels.Region()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }

        public static Sqlite.Models.Region ToModel(this ViewModels.Region source)
        {
            Sqlite.Models.Region result = null;
            if (source != null)
            {
                result = new Sqlite.Models.Region()
                {
                    Name = source.Name,
                    Active = source.Active
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
