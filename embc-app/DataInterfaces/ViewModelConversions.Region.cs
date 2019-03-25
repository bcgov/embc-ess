using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Region ToViewModel(this Models.Db.Region source)
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

        public static Models.Db.Region ToModel(this ViewModels.Region source)
        {
            Models.Db.Region result = null;
            if (source != null)
            {
                result = new Models.Db.Region()
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
