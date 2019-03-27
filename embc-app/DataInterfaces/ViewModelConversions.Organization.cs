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
                    Externaluseridentifier = source.Externaluseridentifier,
                    Region = source.Region?.ToViewModel(),
                    RegionalDistrict = source.RegionalDistrict?.ToViewModel(),
                    Community = source.Community?.ToViewModel()
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
                    Externaluseridentifier = source.Externaluseridentifier,
                    RegionId = source.Region == null ? (Guid?)null : Guid.Parse(source.Region.Id),
                    RegionalDistrictId = source.RegionalDistrict == null ? (Guid?)null : Guid.Parse(source.RegionalDistrict?.Id),
                    CommunityId = source.Community == null ? (Guid?)null : Guid.Parse(source.Community?.Id)
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                if (source.Active.HasValue)
                {
                    result.Active = source.Active.Value;
                }
            }
            return result;
        }
    }
}
