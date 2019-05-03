using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static Models.Db.IncidentRegistrationAddress ToModel(this ViewModels.Address source, AddressType addressType)
        {
            Models.Db.IncidentRegistrationAddress result = null;
            if (source != null)
            {
                result = new Models.Db.IncidentRegistrationAddress
                {
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    PostalCode = source.PostalCode,
                    Province = source.Province,
                    CountryCode = source.Country.CountryCode
                };

                if (source.Id != null)
                {
                    result.IncidentRegistrationId = Guid.Parse(source.Id);
                }

                result.AddressTypeCode = addressType.GetDisplayName();
                result.AddressSequenceNumber = addressType == AddressType.Primary ? 1 : 2;

                if (source.isBcAddress)
                {
                    result.AddressSubtypeCode = AddressSubType.BCAddress.GetDisplayName();
                    result.CommunityId = Guid.Parse(source.Community.Id);
                }
                if (source.isOtherAddress)
                {
                    result.AddressSubtypeCode = AddressSubType.OtherAddress.GetDisplayName();
                    result.City = source.City;
                }
            }
            return result;
        }
    }
}
