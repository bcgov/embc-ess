using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class CommunityMappingProfile : Profile
    {
        public CommunityMappingProfile()
        {
            CreateMap<Models.Db.Community, Community>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.Id.ToString()))
                .ReverseMap();
        }
    }

    public class Community
    {
        public string Id { get; set; }
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.
        public string Name { get; set; }

        // related entities
        public Region Region { get; set; }
    }
}
