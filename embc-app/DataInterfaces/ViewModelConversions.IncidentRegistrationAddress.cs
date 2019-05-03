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
        public static ViewModels.Address ToViewModel(this Models.Db.IncidentRegistrationAddress source)
        {
            ViewModels.Address result = null;
            if (source != null)
            {
                result = new ViewModels.Address();
                result.Id = source.IncidentRegSeqId;
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.PostalCode = source.PostalCode;
                result.Province = source.Province;
                result.Country = source.Country?.ToViewModel();
                result.AddressSubtype = source.AddressSubtypeCode;
                result.Community = source.Community?.ToViewModel();
                result.City = source.City;
            }
            return result;
        }

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
                    result.IncidentRegistrationId = Models.Db.IncidentRegistrationAddress.GetIncidentRegistrationIdFromIncidentRegSeqId(source.Id);
                    result.AddressSequenceNumber = Models.Db.IncidentRegistrationAddress.GetAddressSequenceNumberFromIncidentRegSeqId(source.Id);
                }
                else
                {
                    result.AddressSequenceNumber = addressType == AddressType.Primary ? 1 : 2;
                }

                result.AddressTypeCode = addressType.GetDisplayName();

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
