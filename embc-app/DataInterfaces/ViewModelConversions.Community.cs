namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Community ToViewModel(this Models.Db.Community source)
        {
            ViewModels.Community result = null;
            if (source != null)
            {
                result = new ViewModels.Community()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active,
                    Region = source.Region.ToViewModel()
                };
            }
            return result;
        }
    }
}
