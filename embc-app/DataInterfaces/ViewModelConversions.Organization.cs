using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Organization ToViewModel(this Sqlite.Models.Organization source)
        {
            ViewModels.Organization result = null;
            if (source != null)
            {
                result = new ViewModels.Organization()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    BceidAccountNumber = source.BceidAccountNumber,
                    Active = source.Active
                };
            }
            return result;
        }

        public static Sqlite.Models.Organization ToModel(this ViewModels.Organization source)
        {
            Sqlite.Models.Organization result = null;
            if (source != null)
            {
                result = new Sqlite.Models.Organization()
                {
                    Name = source.Name,
                    BceidAccountNumber = source.BceidAccountNumber,
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
