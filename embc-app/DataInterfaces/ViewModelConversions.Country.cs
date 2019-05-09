namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Country ToViewModel(this Models.Db.Country source)
        {
            var result = new ViewModels.Country
            {
                Id = source.CountryCode,
                CountryCode = source.CountryCode,
                Name = source.Name,
                Active = source.Active
            };
            return result;
        }
    }
}
