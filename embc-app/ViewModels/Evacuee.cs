using AutoMapper;
using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class EvacueeMappingProfie : Profile
    {
        public EvacueeMappingProfie()
        {
            CreateMap<Models.Db.Evacuee, EvacueeListItem>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.RegistrationIdSeq))
                .ForMember(d => d.IsHeadOfHousehold, opts => opts.MapFrom(s => s.EvacueeType == Models.Db.Enumerations.EvacueeType.HeadOfHousehold))
                .ForMember(d => d.RestrictedAccess, opts => opts.MapFrom(s => s.EvacueeRegistration.RestrictedAccess))
                .ForMember(d => d.IncidentTaskNumber, opts => opts.MapFrom(s => s.EvacueeRegistration.IncidentTask.TaskNumber))
                .ForMember(d => d.RegistrationCompletionDate, opts => opts.MapFrom(s => s.EvacueeRegistration.RegistrationCompletionDate))
                .ForMember(d => d.EvacuatedFrom, opts => opts.MapFrom(s => s.EvacueeRegistration.IncidentTask.Community != null
                    ? s.EvacueeRegistration.IncidentTask.Community.Name
                    : s.EvacueeRegistration.IncidentTask.Region.Name))
                .ForMember(d => d.EvacuatedTo, opts => opts.MapFrom(s => s.EvacueeRegistration.HostCommunity.Name))
                ;
        }
    }

    public class Evacuee : Person
    {
    }

    public class EvacueeListItem
    {
        public string Id { get; set; }
        public bool RestrictedAccess { get; set; }
        public bool IsHeadOfHousehold { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Initials { get; set; }
        public string RegistrationId { get; set; }
        public string IncidentTaskNumber { get; set; }
        public string EvacuatedFrom { get; set; }
        public string EvacuatedTo { get; set; }
        public DateTime? RegistrationCompletionDate { get; set; }
    }
}
