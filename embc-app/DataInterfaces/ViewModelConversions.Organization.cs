using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Organization ToViewModel(this Models.Db.Organization source)
        {
            ViewModels.Organization result = null;
            if (source != null)
            {
                result = new ViewModels.Organization()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    BceidAccountNumber = source.BceidAccountNumber,
                    Active = source.Active,
                    Externaluseridentifier = source.Externaluseridentifier
                };
            }
            return result;
        }

        public static Models.Db.Organization ToModel(this ViewModels.Organization source)
        {
            Models.Db.Organization result = null;
            if (source != null)
            {
                result = new Models.Db.Organization()
                {
                    Name = source.Name,
                    BceidAccountNumber = source.BceidAccountNumber,
                    Active = source.Active,
                    Externaluseridentifier = source.Externaluseridentifier
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
