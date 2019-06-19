using AutoMapper;
using System;
using System.Linq;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class IncidentTaskMappingProfile : Profile
    {
        public IncidentTaskMappingProfile()
        {
            CreateMap<IncidentTask, Models.Db.IncidentTask>()
                .ForMember(d => d.EvacueeRegistrations, opts => opts.Ignore())
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate.HasValue ? new DateTimeOffset(s.StartDate.Value) : (DateTimeOffset?)null))
                .ForMember(d => d.RegionName, opts => opts.MapFrom(s => s.Region.Name))
                .ForMember(d => d.CommunityId, opts => opts.MapFrom(s => s.Community.Id))
                .ForMember(d => d.Region, opts => opts.Ignore())
                .ForMember(d => d.Community, opts => opts.Ignore())
                .ReverseMap()
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate.HasValue ? s.StartDate.Value.DateTime : (DateTime?)null))
                .ForMember(d => d.TotalAssociatedEvacuees, opts => opts.MapFrom(s => s.EvacueeRegistrations.Count()))
            ;
        }
    }

    public class IncidentTask
    {
        public string Id { get; set; }
        public string TaskNumber { get; set; }
        public string Details { get; set; }
        public bool? Active { get; set; }
        public int? TotalAssociatedEvacuees { get; set; }
        public Region Region { get; set; }
        public Community Community { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
