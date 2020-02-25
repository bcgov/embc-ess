using System;
using AutoMapper;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class VolunteerTaskMappingProfile : Profile
    {
        public VolunteerTaskMappingProfile()
        {
            // CreateMap<Volunteer, Models.Db.VolunteerTask>()
            //     .ForMember(d => d.VolunteerId, opts => opts.MapFrom(s => s.Id))
            //     .ForMember(d => d.Organization, opts => opts.Ignore())
            //     .ForMember(d => d.BceidAccountUserName, opts => opts.MapFrom(s => s.BceidAccountNumber))
            //     .ForMember(d => d.BCeId, opts => opts.MapFrom(s => s.Externaluseridentifier))
            //     .ForMember(d => d.IsNewUser, opts => opts.Ignore())
            //     .ReverseMap()
            //     ;
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
