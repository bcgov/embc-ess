using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<Models.Db.Region, Region>()
                .ReverseMap();
        }
    }

    public class Region
    {
        public bool Active { get; set; }
        public string Name { get; set; }
    }
}
