using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        //public static ViewModels.Address ToViewModel(this Models.Db.Address source)
        //{
        //    ViewModels.Address result = null;
        //    if (source != null)
        //    {
        //        result = new ViewModels.Address();
        //        result.Id = source.Id.ToString();
        //        result.AddressLine1 = source.AddressLine1;
        //        result.AddressLine2 = source.AddressLine2;
        //        result.AddressLine3 = source.AddressLine3;
        //        result.PostalCode = source.PostalCode;
        //        result.Province = source.Province;
        //        result.Country = source.Country?.ToViewModel();

        //        if (source is Models.Db.BcAddress sourceBc)
        //        {
        //            result.AddressSubtype = Models.Db.Address.BC_ADDRESS;
        //            result.Community = sourceBc.Community.ToViewModel();
        //        }
        //        if (source is Models.Db.OtherAddress sourceOther)
        //        {
        //            result.AddressSubtype = Models.Db.Address.OTHER_ADDRESS;
        //            result.City = sourceOther.City;
        //        }
        //    }
        //    return result;
        //}

        //public static Models.Db.Address ToModel(this ViewModels.Address source)
        //{
        //    Models.Db.Address result = null;
        //    if (source != null)
        //    {
        //        result = Models.Db.Address.Create(source.AddressSubtype);
        //        result.AddressLine1 = source.AddressLine1;
        //        result.AddressLine2 = source.AddressLine2;
        //        result.AddressLine3 = source.AddressLine3;
        //        result.PostalCode = source.PostalCode;
        //        result.Province = source.Province;
        //        result.CountryCode = source.Country.CountryCode;

        //        if (source.Id != null)
        //        {
        //            result.Id = Guid.Parse(source.Id);
        //        }
        //        if (source.isBcAddress)
        //        {
        //            var resultBc = result as Models.Db.BcAddress;
        //            resultBc.CommunityId = Guid.Parse(source.Community.Id);
        //        }
        //        if (source.isOtherAddress)
        //        {
        //            var resultOther = result as Models.Db.OtherAddress;
        //            resultOther.City = source.City;
        //        }
        //    }
        //    return result;
        //}
    }
}
