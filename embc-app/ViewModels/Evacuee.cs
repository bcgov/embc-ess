using AutoMapper;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Utils;
using System;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class EvacueeMappingProfie : Profile
    {
        public EvacueeMappingProfie()
        {
            CreateMap<Models.Db.Evacuee, EvacueeListItem>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => $"{s.RegistrationId.ToString()}-{s.EvacueeSequenceNumber}"))
                .ForMember(d => d.IsHeadOfHousehold, opts => opts.MapFrom(s => s.EvacueeType == Models.Db.Enumerations.EvacueeType.HeadOfHousehold))
                .ForMember(d => d.RestrictedAccess, opts => opts.MapFrom(s => s.EvacueeRegistration.RestrictedAccess))
                .ForMember(d => d.IncidentTaskNumber, opts => opts.MapFrom(s => s.EvacueeRegistration.IncidentTask.TaskNumber))
                .ForMember(d => d.RegistrationCompletionDate, opts => opts.MapFrom(s => s.EvacueeRegistration.RegistrationCompletionDate))
                .ForMember(d => d.EvacuatedFrom, opts => opts.MapFrom(s => s.EvacueeRegistration.HostCommunity.Name))
                .ForMember(d => d.EvacuatedTo, opts => opts.MapFrom(s => s.EvacueeRegistration.IncidentTask.Community != null
                                                                        ? s.EvacueeRegistration.IncidentTask.Community.Name
                                                                        : s.EvacueeRegistration.IncidentTask.Region.Name))
                .ForMember(d => d.HasReferrals, opts => opts.MapFrom(s => s.EvacueeRegistration.RegistrationCompletionDate.HasValue
                ? s.Referrals.Any()
                : (bool?)null))
                .ForMember(d => d.IsFinalized, opts => opts.MapFrom(s => s.EvacueeRegistration.RegistrationCompletionDate.HasValue))
                ;

            CreateMap<Models.Db.ViewEvacuee, EvacueeListItem>();

            CreateMap<EvacueeType, FamilyRelationshipType>()
                .ForMember(x => x.Active, opts => opts.MapFrom(s => true))
                .ForMember(x => x.Code, opts => opts.MapFrom(s => s.GetDisplayName()))
                .ForMember(x => x.Description, opts => opts.MapFrom(s => s.GetDescription()));
        }
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
        public bool IsFinalized { get; set; }
        public bool? HasReferrals { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? SelfRegisteredDate { get; set; }
        public string PrimaryAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }


    }
}
