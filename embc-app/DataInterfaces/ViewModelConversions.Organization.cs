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
                    RegionId = source.RegionId,
                    Region = source.Region.ToViewModel(),
                    RegionalDistrictId = source.RegionalDistrictId,
                    RegionalDistrict = source.RegionalDistrict.ToViewModel(),
                    CommunityId = source.CommunityId,
                    Community = source.Community.ToViewModel()
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
                    Externaluseridentifier = source.Externaluseridentifier,
                    RegionId = source.RegionId,
                    RegionalDistrictId = source.RegionalDistrictId,
                    CommunityId = source.CommunityId
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
