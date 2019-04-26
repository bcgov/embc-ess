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
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }
    }
}
