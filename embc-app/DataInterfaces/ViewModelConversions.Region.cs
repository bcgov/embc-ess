namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Region ToViewModel(this Models.Db.Region source)
        {
            ViewModels.Region result = null;
            if (source != null)
            {
                result = new ViewModels.Region()
                {
                    Id = source.Name,
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }
    }
}
