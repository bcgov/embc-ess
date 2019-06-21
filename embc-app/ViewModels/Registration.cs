using AutoMapper;
using Gov.Jag.Embc.Public.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class RegistrationMappingProfile : Profile
    {
        public RegistrationMappingProfile()
        {
            CreateMap<Registration, Models.Db.EvacueeRegistration>()
                .ForMember(d => d.EssFileNumber, opts => opts.MapFrom(s => s.EssFileNumber))
                .ForMember(d => d.CompletedById, opts => opts.MapFrom(s => s.CompletedBy.Id))
                .ForMember(d => d.IncidentTaskId, opts => opts.MapFrom(s => s.IncidentTask.Id))
                .ForMember(d => d.IncidentTask, opts => opts.Ignore())
                .ForMember(d => d.HostCommunityId, opts => opts.MapFrom(s => s.HostCommunity.Id))
                .ForMember(d => d.HostCommunity, opts => opts.Ignore())
                .ForMember(d => d.PhoneNumber, opts => opts.MapFrom(s => s.HeadOfHousehold.PhoneNumber))
                .ForMember(d => d.PhoneNumberAlt, opts => opts.MapFrom(s => s.HeadOfHousehold.PhoneNumberAlt))
                .ForMember(d => d.Email, opts => opts.MapFrom(s => s.HeadOfHousehold.Email))
                .ForMember(d => d.FollowUpDetails, opts => opts.MapFrom(s => s.InternalCaseNotes))
                .ForMember(d => d.Evacuees, opts => opts.MapFrom(s => s.HeadOfHousehold.FamilyMembers.Cast<Evacuee>().Prepend(s.HeadOfHousehold)))
                .AfterMap((s, d) =>
                {
                    var seq = 1;
                    foreach (var evacuee in d.Evacuees)
                    {
                        if (s.EssFileNumber.HasValue) evacuee.RegistrationId = s.EssFileNumber.Value;
                        evacuee.EvacueeSequenceNumber = seq;
                        seq++;
                    }
                })
                .ForMember(d => d.EvacueeRegistrationAddresses, opts => opts.MapFrom(s => (new Address[] { s.HeadOfHousehold.PrimaryResidence, s.HeadOfHousehold.MailingAddress }).Where(a => a != null)))
                .AfterMap((s, d) =>
                {
                    var seq = 1;
                    foreach (var address in d.EvacueeRegistrationAddresses)
                    {
                        if (s.EssFileNumber.HasValue) address.RegistrationId = s.EssFileNumber.Value;
                        address.AddressSequenceNumber = seq++;
                        address.AddressTypeCode = (address.AddressSequenceNumber == 1 ? AddressType.Primary : AddressType.Mailing).GetDisplayName();
                    }
                })
                ;

            CreateMap<Models.Db.EvacueeRegistration, Registration>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.EssFileNumber))
                .ForMember(d => d.HeadOfHousehold, opts => opts.MapFrom(s => s.Evacuees.Single(e => e.EvacueeType == EvacueeType.HeadOfHousehold)))
                .ForMember(d => d.InternalCaseNotes, opts => opts.MapFrom(s => s.FollowUpDetails))
                .AfterMap((s, d, ctx) =>
                {
                    d.HeadOfHousehold.Email = s.Email;
                    d.HeadOfHousehold.PhoneNumber = s.PhoneNumber;
                    d.HeadOfHousehold.PhoneNumberAlt = s.PhoneNumberAlt;
                    d.HeadOfHousehold.PrimaryResidence = ctx.Mapper.Map<Address>(s.EvacueeRegistrationAddresses.Single(a => a.AddressType == AddressType.Primary));
                    var mailingAddress = s.EvacueeRegistrationAddresses.SingleOrDefault(a => a.AddressType == AddressType.Mailing);
                    if (mailingAddress != null) d.HeadOfHousehold.MailingAddress = ctx.Mapper.Map<Address>(mailingAddress);
                    d.HeadOfHousehold.FamilyMembers = ctx.Mapper.Map<IEnumerable<FamilyMember>>(s.Evacuees.Where(e => e.EvacueeType != EvacueeType.HeadOfHousehold));
                })
                .ForMember(d => d.CompletedBy, opts => opts.Ignore())
                ;

            CreateMap<Evacuee, Models.Db.Evacuee>(MemberList.None)
                .ForMember(d => d.RegistrationId, opts => opts.MapFrom(s => s.Id.Split("-", StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(0)))
                .ForMember(d => d.EvacueeSequenceNumber, opts => opts.MapFrom(s => s.Id.Split("-", StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1)))
                .ForMember(d => d.EvacueeRegistration, opts => opts.Ignore())
                .ForMember(d => d.BcServicesNumber, opts => opts.Ignore())
                .ForMember(d => d.Referrals, opts => opts.Ignore())
                .ForMember(d => d.EvacueeTypeCode, opts => opts.Ignore())
                .IncludeAllDerived()
                .ReverseMap()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => $"{s.RegistrationId}-{s.EvacueeSequenceNumber}"))
                 ;

            CreateMap<HeadOfHousehold, Models.Db.Evacuee>()
                .IncludeBase<Evacuee, Models.Db.Evacuee>()
                .ForMember(d => d.EvacueeTypeCode, opts => opts.MapFrom(s => EvacueeType.HeadOfHousehold.GetDisplayName()))
                .ForMember(d => d.SameLastNameAsEvacuee, opts => opts.Ignore())
                .ReverseMap()
                .IncludeBase<Models.Db.Evacuee, Evacuee>()
                .ForMember(d => d.PersonType, opts => opts.MapFrom(s => Evacuee.HOH))
                .ForMember(d => d.FamilyMembers, opts => opts.Ignore())
                .ForMember(d => d.PhoneNumber, opts => opts.Ignore())
                .ForMember(d => d.PhoneNumberAlt, opts => opts.Ignore())
                .ForMember(d => d.Email, opts => opts.Ignore())
                ;

            CreateMap<FamilyMember, Models.Db.Evacuee>()
                .IncludeBase<Evacuee, Models.Db.Evacuee>()
                .ForMember(d => d.SameLastNameAsEvacuee, opts => opts.MapFrom(s => s.SameLastNameAsEvacuee))
                .ForMember(d => d.EvacueeTypeCode, opts => opts.MapFrom(s => s.RelationshipToEvacuee.Code))
                .ReverseMap()
                .IncludeBase<Models.Db.Evacuee, Evacuee>()
                .ForMember(d => d.PersonType, opts => opts.MapFrom(s => Evacuee.FAMILY_MEMBER))
                .ForMember(d => d.RelationshipToEvacuee, opts => opts.MapFrom(s => s.EvacueeType))
                ;

            CreateMap<Address, Models.Db.EvacueeRegistrationAddress>()
                .ForMember(d => d.CountryCode, opts => opts.MapFrom(s => s.Country.CountryCode))
                .ForMember(d => d.Country, opts => opts.Ignore())
                .ForMember(d => d.CommunityId, opts => opts.MapFrom(s => s.Community.Id))
                .ForMember(d => d.Community, opts => opts.Ignore())
                .ForMember(d => d.RegistrationId, opts => opts.Ignore())
                .ForMember(d => d.AddressSequenceNumber, opts => opts.Ignore())
                .ForMember(d => d.AddressTypeCode, opts => opts.Ignore())
                .ForMember(d => d.AddressSubtypeCode, opts => opts.MapFrom(s => s.AddressSubtype))
                .ReverseMap()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => $"{s.RegistrationId}-{s.AddressSequenceNumber}"))
                ;
        }
    }

    public class Registration
    {
        public string Id { get; set; }
        public bool? Active { get; set; }

        // important
        public bool RestrictedAccess { get; set; }

        public bool? DeclarationAndConsent { get; set; }
        public long? EssFileNumber { get; set; }

        // registration record
        public bool? DietaryNeeds { get; set; }

        public string DietaryNeedsDetails { get; set; }
        public string DisasterAffectDetails { get; set; }
        public string ExternalReferralsDetails { get; set; }
        public string Facility { get; set; }
        public string FamilyRecoveryPlan { get; set; }
        public string InternalCaseNotes { get; set; }
        public string InsuranceCode { get; set; }  // one of ['yes', 'yes-unsure', 'no', 'unsure']
        public bool? MedicationNeeds { get; set; }
        public DateTime? SelfRegisteredDate { get; set; }
        public DateTime? RegistrationCompletionDate { get; set; }
        public string RegisteringFamilyMembers { get; set; }  // one of ['yes', 'yes-later', 'no']

        // family state flags
        public bool? HasThreeDayMedicationSupply { get; set; }

        public bool? HasInquiryReferral { get; set; }
        public bool? HasHealthServicesReferral { get; set; }
        public bool? HasFirstAidReferral { get; set; }
        public bool? HasChildCareReferral { get; set; }
        public bool? HasPersonalServicesReferral { get; set; }
        public bool? HasPetCareReferral { get; set; }
        public bool? HasPets { get; set; }

        // requirements
        public bool? RequiresAccommodation { get; set; }

        public bool? RequiresClothing { get; set; }
        public bool? RequiresFood { get; set; }
        public bool? RequiresIncidentals { get; set; }
        public bool? RequiresTransportation { get; set; }
        public bool? RequiresSupport { get; set; }

        // related entities
        public HeadOfHousehold HeadOfHousehold { get; set; }

        public IncidentTask IncidentTask { get; set; }
        public Community HostCommunity { get; set; }

        public Volunteer CompletedBy { get; set; }

        public bool IsFinalized { get; set; }
    }

    public abstract class Evacuee
    {
        public const string HOH = "HOH";
        public const string FAMILY_MEMBER = "FMBR";

        public string Id { get; set; }
        public bool? Active { get; set; }  // no deletions from DB this is a soft delete.
        public string PersonType { get; set; }  // one of 'VOLN' (volunteer), 'HOH' (head of household), 'FMBR' (family member)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Initials { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? Dob { get; set; }
    }

    public class HeadOfHousehold : Evacuee
    {
        public string PhoneNumber { get; set; }

        public string PhoneNumberAlt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public Address PrimaryResidence { get; set; }

        public Address MailingAddress { get; set; }

        [Required]
        public IEnumerable<FamilyMember> FamilyMembers { get; set; }

        public HeadOfHousehold()
        {
            PersonType = Evacuee.HOH;
        }
    }

    public class FamilyMember : Evacuee
    {
        public bool SameLastNameAsEvacuee { get; set; }
        public FamilyRelationshipType RelationshipToEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = Evacuee.FAMILY_MEMBER;
        }
    }

    public class Address
    {
        // base props
        public string Id { get; set; }

        public string AddressSubtype { get; set; }  // one of ['BCAD', 'OTAD'] for BC vs non-BC addresses
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PostalCode { get; set; }

        // BC address props
        public Community Community { get; set; }

        // other address props
        public string City { get; set; }

        public string Province { get; set; }
        public Country Country { get; set; }

        [JsonIgnore]
        public bool isBcAddress => this.AddressSubtype == Models.Db.Enumerations.AddressSubType.BCAddress.GetDisplayName();  // omitted from response

        [JsonIgnore]
        public bool isOtherAddress => this.AddressSubtype == Models.Db.Enumerations.AddressSubType.OtherAddress.GetDisplayName();  // omitted from response
    }
}
