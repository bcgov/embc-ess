using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<Models.Db.Country, Country>()
                .ForMember(x => x.Id, x => x.MapFrom(opt => opt.CountryCode))
                .ReverseMap();
        }
    }

    public class Country
    {
        public string Id { get; set; }
        public string CountryCode { get; set; }
        public bool? Active { get; set; }
        public string Name { get; set; }
    }
}
