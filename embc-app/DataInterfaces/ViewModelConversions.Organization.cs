using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Organization ToViewModel(this Models.Db.Organization source, Models.Db.Volunteer primaryContact = null)
        {
            ViewModels.Organization result = null;
            if (source != null)
            {
                result = new ViewModels.Organization()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active,
                    BCeIDBusinessGuid = source.BCeIDBusinessGuid,
                    Region = source.Region?.ToViewModel(),
                    RegionalDistrict = source.RegionalDistrict?.ToViewModel(),
                    Community = source.Community?.ToViewModel(),
                    AdminBCeID = primaryContact?.BceidAccountNumber,
                    AdminFirstName = primaryContact?.FirstName,
                    AdminLastName = primaryContact?.LastName
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
                    BCeIDBusinessGuid = source.BCeIDBusinessGuid,
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
