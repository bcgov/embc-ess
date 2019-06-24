using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class FamilyRelationshipTypeMappingProfile : Profile
    {
        public FamilyRelationshipTypeMappingProfile()
        {
            CreateMap<Models.Db.FamilyRelationshipType, FamilyRelationshipType>()
                .ReverseMap();
        }
    }

    public sealed class FamilyRelationshipType
    {
        public string Code { get; set; }
        public bool? Active { get; set; }
        public string Description { get; set; }
    }
}
