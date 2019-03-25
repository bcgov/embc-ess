using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Community ToViewModel(this Models.Db.Community source)
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

        public static Models.Db.Community ToModel(this ViewModels.Community source)
        {
            Models.Db.Community result = null;
            if (source != null)
            {
                result = new Models.Db.Community()
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
