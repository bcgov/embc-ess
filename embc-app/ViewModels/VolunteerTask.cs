using System;
using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class VolunteerTaskMappingProfile : Profile
    {
        public VolunteerTaskMappingProfile()
        {
            CreateMap<VolunteerTask, Models.Db.VolunteerTask>()
                .ForMember(d => d.VolunteerId, opts => opts.MapFrom(s => s.VolunteerId))
                .ForMember(d => d.IncidentTaskId, opts => opts.MapFrom(s => s.IncidentTaskId))
                .ForMember(d => d.Volunteer, opts => opts.Ignore())
                .ForMember(d => d.IncidentTask, opts => opts.Ignore())
                .ReverseMap()
                ;
        }
    }

    public class VolunteerTask
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public Guid IncidentTaskId { get; set; }

        public DateTimeOffset LastDateVolunteerConfirmedTask { get; set; }

        public Volunteer Volunteer { get; set; }
        public IncidentTask IncidentTask { get; set; }

    }

}
