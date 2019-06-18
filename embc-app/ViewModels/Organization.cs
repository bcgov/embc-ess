using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class OrganizationMappingProfile : Profile
    {
        public OrganizationMappingProfile()
        {
            CreateMap<Organization, Models.Db.Organization>()
                .ForMember(d => d.CommunityId, opts => opts.MapFrom(s => s.Community.Id))
                .ForMember(d => d.RegionName, opts => opts.MapFrom(s => s.Region.Name))
                .ForMember(d => d.Region, opts => opts.Ignore())
                .ForMember(d => d.Community, opts => opts.Ignore())
                .ReverseMap()
                .ForMember(d => d.AdminBCeID, opts => opts.MapFrom((s, d, m, ctx) => ((Models.Db.Volunteer)ctx.Items["Admin"]).BceidAccountUserName))
                .ForMember(d => d.AdminFirstName, opts => opts.MapFrom((s, d, m, ctx) => ((Models.Db.Volunteer)ctx.Items["Admin"]).FirstName))
                .ForMember(d => d.AdminLastName, opts => opts.MapFrom((s, d, m, ctx) => ((Models.Db.Volunteer)ctx.Items["Admin"]).LastName))
                .ForMember(d => d.LegalName, opts => opts.Ignore())
                ;
        }
    }

    public class Organization
    {
        public string Id { get; set; }
        public bool? Active { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        public string BCeIDBusinessGuid { get; set; }

        public Region Region { get; set; }

        public Community Community { get; set; }

        public string AdminFirstName { get; set; }

        public string AdminLastName { get; set; }

        public string AdminBCeID { get; set; }

        public string LegalName { get; set; }
    }
}
