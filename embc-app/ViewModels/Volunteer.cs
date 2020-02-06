using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class VolunteerMappingProfile : Profile
    {
        public VolunteerMappingProfile()
        {
            CreateMap<Volunteer, Models.Db.Volunteer>()
                .ForMember(d => d.OrganizationId, opts => opts.MapFrom(s => s.Organization.Id))
                .ForMember(d => d.Organization, opts => opts.Ignore())
                .ForMember(d => d.BceidAccountUserName, opts => opts.MapFrom(s => s.BceidAccountNumber))
                .ForMember(d => d.BCeId, opts => opts.MapFrom(s => s.Externaluseridentifier))
                .ForMember(d => d.IsNewUser, opts => opts.Ignore())
                .ReverseMap()
                ;
        }
    }

    public class Volunteer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public string BceidAccountNumber { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsPrimaryContact { get; set; }
        public bool? CanAccessRestrictedFiles { get; set; }
        public string Externaluseridentifier { get; set; }
        public Organization Organization { get; set; }
    }
}
