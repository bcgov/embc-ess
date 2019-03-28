namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Country ToViewModel(this Models.Db.Country source)
        {
            ViewModels.Country result = null;
            if (source != null)
            {
                result = new ViewModels.Country()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }
    }
}
