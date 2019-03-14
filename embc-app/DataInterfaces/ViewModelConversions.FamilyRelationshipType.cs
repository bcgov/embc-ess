using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.FamilyRelationshipType ToViewModel(this Sqlite.Models.FamilyRelationshipType source)
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

        public static Sqlite.Models.FamilyRelationshipType ToModel(this ViewModels.FamilyRelationshipType source)
        {
            Sqlite.Models.FamilyRelationshipType result = null;
            if (source != null)
            {
                result = new Sqlite.Models.FamilyRelationshipType()
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
