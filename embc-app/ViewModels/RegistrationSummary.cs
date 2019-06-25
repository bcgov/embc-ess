using AutoMapper;
using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class RegistrationSummaryMappingProfile : Profile
    {
        public RegistrationSummaryMappingProfile()
        {
            CreateMap<Registration, RegistrationSummary>()
                .ForMember(d => d.HasInternalCaseNotes, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.InternalCaseNotes)))
                ;
        }
    }

    public class RegistrationSummary
    {
        public string Id { get; set; }
        public bool? RestrictedAccess { get; set; }
        public long? EssFileNumber { get; set; }
        public DateTime? SelfRegisteredDate { get; set; }
        public DateTime? RegistrationCompletionDate { get; set; }
        public string RegisteringFamilyMembers { get; set; }  // one of ['yes', 'yes-later', 'no']
        public bool? RequiresAccommodation { get; set; }
        public bool? RequiresClothing { get; set; }
        public bool? RequiresFood { get; set; }
        public bool? RequiresIncidentals { get; set; }
        public bool? RequiresTransportation { get; set; }
        public bool? RequiresSupport { get; set; }
        public HeadOfHousehold HeadOfHousehold { get; set; }
        public IncidentTask IncidentTask { get; set; }
        public Community HostCommunity { get; set; }
        public bool Active { get; set; }
        public bool HasInternalCaseNotes { get; set; }
        public string Facility { get; set; }
    }
}
