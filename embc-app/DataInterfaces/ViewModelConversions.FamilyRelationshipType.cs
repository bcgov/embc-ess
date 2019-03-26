namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.FamilyRelationshipType ToViewModel(this Models.Db.FamilyRelationshipType source)
        {
            ViewModels.FamilyRelationshipType result = null;
            if (source != null)
            {
                result = new ViewModels.FamilyRelationshipType()
                {
                    Code = source.Code,
                    Description = source.Description,
                    Active = source.Active
                };
            }
            return result;
        }

        public static Models.Db.FamilyRelationshipType ToModel(this ViewModels.FamilyRelationshipType source)
        {
            Models.Db.FamilyRelationshipType result = null;
            if (source != null)
            {
                result = new Models.Db.FamilyRelationshipType()
                {
                    Code = source.Code,
                    Description = source.Description,
                    Active = source.Active
                };
            }
            return result;
        }
    }
}
