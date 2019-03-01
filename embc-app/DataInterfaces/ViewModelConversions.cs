
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ViewModelConversions
    {
        public static ViewModels.Region ToViewModel(this Sqlite.Models.Region item)
        {
            ViewModels.Region result = null;
            if (item != null)
            {
                result = new ViewModels.Region()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active
                };
            }
            return result;
        }
    }
}
