using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Country ToViewModel(this Sqlite.Models.Country source)
        {
            ViewModels.Country result = null;
            if (source != null)
            {
                result = new ViewModels.Country()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }

        public static Sqlite.Models.Country ToModel(this ViewModels.Country source)
        {
            Sqlite.Models.Country result = null;
            if (source != null)
            {
                result = new Sqlite.Models.Country()
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
