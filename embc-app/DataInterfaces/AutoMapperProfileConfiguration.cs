using AutoMapper;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public class LookupDataAutoMapperProfile : Profile
    {
        public LookupDataAutoMapperProfile()
        {
            CreateMap<Models.Db.Community, ViewModels.Community>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.Id.ToString()));

            CreateMap<Models.Db.Region, ViewModels.Region>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.Name));
        }
    }
}
