using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Country ToViewModel(this Models.Db.Country source)
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

        public static Models.Db.Country ToModel(this ViewModels.Country source)
        {
            Models.Db.Country result = null;
            if (source != null)
            {
                result = new Models.Db.Country()
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
