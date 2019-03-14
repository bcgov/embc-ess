using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Address ToViewModel(this Sqlite.Models.Address source)
        {
            ViewModels.Address result = null;
            if (source != null)
            {
                result = ViewModels.Address.Create(source.AddressSubtype);
                result.Id = source.Id.ToString();
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.PostalCode = source.PostalCode;

                if (source is Sqlite.Models.BcAddress sourceBc)
                {
                    var resultBc = result as ViewModels.BcAddress;
                    resultBc.Community = sourceBc.Community.ToViewModel();
                }
                if (source is Sqlite.Models.OtherAddress sourceOther)
                {
                    var resultOther = result as ViewModels.OtherAddress;
                    resultOther.City = sourceOther.City;
                    resultOther.Province = sourceOther.Province;
                    resultOther.Country = sourceOther.Country.ToViewModel();
                }
            }
            return result;
        }

        public static Sqlite.Models.Address ToModel(this ViewModels.Address source)
        {
            Sqlite.Models.Address result = null;
            if (source != null)
            {
                result = Sqlite.Models.Address.Create(source.AddressSubtypeCode);
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.PostalCode = source.PostalCode;
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                if (source is ViewModels.BcAddress sourceBc)
                {
                    var resultBc = result as Sqlite.Models.BcAddress;
                    resultBc.Community = sourceBc.Community.ToModel();
                }
                if (source is ViewModels.OtherAddress sourceOther)
                {
                    var resultOther = result as Sqlite.Models.OtherAddress;
                    resultOther.City = sourceOther.City;
                    resultOther.Province = sourceOther.Province;
                    resultOther.Country = sourceOther.Country.ToModel();
                }
            }
            return result;
        }
    }
}
