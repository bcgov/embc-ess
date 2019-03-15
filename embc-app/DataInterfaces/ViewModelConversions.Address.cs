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
                result = new ViewModels.Address();
                result.Id = source.Id.ToString();
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.PostalCode = source.PostalCode;

                if (source is Sqlite.Models.BcAddress sourceBc)
                {
                    // var resultBc = result as ViewModels.BcAddress;
                    result.Community = sourceBc.Community.ToViewModel();
                }
                if (source is Sqlite.Models.OtherAddress sourceOther)
                {
                    // var resultOther = result as ViewModels.OtherAddress;
                    result.City = sourceOther.City;
                    result.Province = sourceOther.Province;
                    result.Country = sourceOther.Country.ToViewModel();
                }
            }
            return result;
        }

        public static Sqlite.Models.Address ToModel(this ViewModels.Address source)
        {
            Sqlite.Models.Address result = null;
            if (source != null)
            {
                result = Sqlite.Models.Address.Create(source.AddressSubtype);
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.PostalCode = source.PostalCode;
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                if (source.isBcAddress)
                {
                    var resultBc = result as Sqlite.Models.BcAddress;
                    resultBc.Community = source.Community.ToModel();
                }
                if (source.isOtherAddress)
                {
                    var resultOther = result as Sqlite.Models.OtherAddress;
                    resultOther.City = source.City;
                    resultOther.Province = source.Province;
                    resultOther.Country = source.Country.ToModel();
                }
            }
            return result;
        }
    }
}
