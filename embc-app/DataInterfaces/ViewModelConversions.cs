
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ViewModelConversions
    {

        public static ViewModels.Address ToViewModel(this Sqlite.Models.Address item)
        {
            ViewModels.Address result = null;
            if (item != null)
            {
                result = new ViewModels.Address()
                {
                    Id = item.Id.ToString(),
                    AddressLine1 = item.AddressLine1,
                    AddressLine2 = item.AddressLine2,
                    AddressLine3 = item.AddressLine3,
                    CommunityOrCity = item.CommunityOrCity,
                    Province = item.Province,
                    PostalCode = item.PostalCode,
                    Country = item.Country
                };
            }
            return result;
        }

        public static Sqlite.Models.Address ToModel(this ViewModels.Address item)
        {
            Sqlite.Models.Address result = null;
            if (item != null)
            {
                result = new Sqlite.Models.Address()
                {                    
                    AddressLine1 = item.AddressLine1,
                    AddressLine2 = item.AddressLine2,
                    AddressLine3 = item.AddressLine3,
                    CommunityOrCity = item.CommunityOrCity,
                    Province = item.Province,
                    PostalCode = item.PostalCode,
                    Country = item.Country
                };
                if (item.Id != null)
                {
                    result.Id = Guid.Parse(item.Id);
                }
            }
            return result;
        }

        public static ViewModels.Community ToViewModel(this Sqlite.Models.Community item)
        {
            ViewModels.Community result = null;
            if (item != null)
            {
                result = new ViewModels.Community()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active,
                    RegionalDistrict = item.RegionalDistrict.ToViewModel()
                };
            }
            return result;
        }

        public static ViewModels.Country ToViewModel(this Sqlite.Models.Country item)
        {
            ViewModels.Country result = null;
            if (item != null)
            {
                result = new ViewModels.Country()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active                    
                };
            }
            return result;
        }

        public static Sqlite.Models.Person ToModel(this ViewModels.Person item)
        {
            Sqlite.Models.Person result = null;
            if (item != null)
            {
                result = new Sqlite.Models.Person()
                {
                    Id = Guid.Parse(item.Id),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember
                };
            }
            return result;
        }

        public static ViewModels.Person ToViewModel(this Sqlite.Models.Person item)
        {
            ViewModels.Person result = null;
            if (item != null)
            {
                result = new ViewModels.Person()
                {
                    Id = item.Id.ToString(),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember
                };
            }
            return result;
        }

        public static Sqlite.Models.FamilyMember ToModel(this ViewModels.FamilyMember item)
        {
            Sqlite.Models.FamilyMember result = null;
            if (item != null)
            {
                result = new Sqlite.Models.FamilyMember()
                {
                    Id = Guid.Parse(item.Id),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    RelationshipToEvacuee = item.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = item.SameLastNameAsEvacuee,
                };
            }
            return result;
        }

        public static ViewModels.FamilyMember ToViewModel(this Sqlite.Models.FamilyMember item)
        {
            ViewModels.FamilyMember result = null;
            if (item != null)
            {
                result = new ViewModels.FamilyMember()
                {
                    Id = item.Id.ToString(),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    RelationshipToEvacuee = item.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = item.SameLastNameAsEvacuee,
                };
            }
            return result;
        }

        public static ViewModels.HeadOfHousehold ToViewModel(this Sqlite.Models.HeadOfHousehold item)
        {
            ViewModels.HeadOfHousehold result = null;
            if (item != null)
            {
                result = new ViewModels.HeadOfHousehold()
                {
                    Id = item.Id.ToString(),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    PersonType = "HOH",
                    PhoneNumber = item.PhoneNumber,
                    PhoneNumberAlt = item.PhoneNumberAlt,
                    Email = item.Email,

                    PrimaryResidence = item.PrimaryResidence.ToViewModel(),
                    MailingAddress = item.MailingAddress.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.HeadOfHousehold ToModel(this ViewModels.HeadOfHousehold item)
        {
            Sqlite.Models.HeadOfHousehold result = null;
            if (item != null)
            {
                result = new Sqlite.Models.HeadOfHousehold()
                {                    
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    PersonType = "HOH",
                    PhoneNumber = item.PhoneNumber,
                    PhoneNumberAlt = item.PhoneNumberAlt,
                    Email = item.Email,

                    PrimaryResidence = item.PrimaryResidence.ToModel(),
                    MailingAddress = item.MailingAddress.ToModel()
                };
                if (item.Id != null)
                {
                    result.Id = Guid.Parse(item.Id);
                }
            }
            return result;
        }

        public static ViewModels.Region ToViewModel(this Sqlite.Models.Region item)
        {
            ViewModels.Region result = null;
            if (item != null)
            {
                result = new ViewModels.Region()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active
                };
            }
            return result;
        }

        public static ViewModels.Registration ToViewModel(this Sqlite.Models.Registration item)
        {
            ViewModels.Registration result = null;
            if (item != null)
            {
                result = new ViewModels.Registration()
                {
                    Id = item.Id.ToString(),
                    EssFileNumber = item.EssFileNumber,
                    IsRestrictedAccess = item.IsRestrictedAccess,
                    IsRegisteringFamilyMembers = item.IsRegisteringFamilyMembers,
                    HasDietaryNeeds = item.HasDietaryNeeds,
                    DietaryNeedsDetails = item.DietaryNeedsDetails,
                    IsTakingMedication = item.IsTakingMedication,
                    HasThreeDaySupply = item.HasThreeDaySupply,
                    HasPets = item.HasPets,
                    InsuranceCode = item.InsuranceCode,
                    IsSupportRequired = item.IsSupportRequired,
                    RequiresFood = item.RequiresFood,
                
                    RequiresClothing = item.RequiresClothing,
                    RequiresAccommodation = item.RequiresAccommodation,
                    RequiresIncidentals = item.RequiresIncidentals,
                    RequiresTransportation = item.RequiresTransportation,
                    Facility = item.Facility,
                    DisasterAffectDetails = item.DisasterAffectDetails,
                    ExternalReferralsDetails = item.ExternalReferralsDetails,
                    FamilyRecoveryPlan = item.FamilyRecoveryPlan,
                    FollowUpDetails = item.FollowUpDetails,
                    HasInquiryReferral = item.HasInquiryReferral,
                    HasHealthServicesReferral = item.HasHealthServicesReferral,
                    HasFirstAidReferral = item.HasFirstAidReferral,
                    HasPersonalServicesReferral = item.HasPersonalServicesReferral,
                    HasChildCareReferral = item.HasChildCareReferral,
                    HasPetCareReferral = item.HasPetCareReferral,
                    SelfRegisteredDate = item.SelfRegisteredDate,
                    RegistrationCompletionDate = item.RegistrationCompletionDate,
                    HeadOfHousehold = item.HeadOfHousehold.ToViewModel(),


                    InterviewerFirstName = item.InterviewerFirstName,
                    InterviewerLastNameInitial = item.InterviewerLastNameInitial,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,

        //specialNeeds: {};

                };
                
                if (item.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<ViewModels.FamilyMember>();
                    foreach (var familyMember in item.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToViewModel());
                    }
                }
            }


            return result;
        }

        public static Sqlite.Models.Registration ToModel(this ViewModels.Registration item)
        {
            Sqlite.Models.Registration result = null;
            if (item != null)
            {
                result = new Sqlite.Models.Registration()
                {
                    Id = Guid.Parse(item.Id),
                    EssFileNumber = item.EssFileNumber,
                    IsRestrictedAccess = item.IsRestrictedAccess,
                    IsRegisteringFamilyMembers = item.IsRegisteringFamilyMembers,
                    HasDietaryNeeds = item.HasDietaryNeeds,
                    DietaryNeedsDetails = item.DietaryNeedsDetails,
                    IsTakingMedication = item.IsTakingMedication,
                    HasThreeDaySupply = item.HasThreeDaySupply,
                    HasPets = item.HasPets,
                    InsuranceCode = item.InsuranceCode,
                    IsSupportRequired = item.IsSupportRequired,
                    RequiresFood = item.RequiresFood,

                    RequiresClothing = item.RequiresClothing,
                    RequiresAccommodation = item.RequiresAccommodation,
                    RequiresIncidentals = item.RequiresIncidentals,
                    RequiresTransportation = item.RequiresTransportation,
                    Facility = item.Facility,
                    DisasterAffectDetails = item.DisasterAffectDetails,
                    ExternalReferralsDetails = item.ExternalReferralsDetails,
                    FamilyRecoveryPlan = item.FamilyRecoveryPlan,
                    FollowUpDetails = item.FollowUpDetails,
                    HasInquiryReferral = item.HasInquiryReferral,
                    HasHealthServicesReferral = item.HasHealthServicesReferral,
                    HasFirstAidReferral = item.HasFirstAidReferral,
                    HasPersonalServicesReferral = item.HasPersonalServicesReferral,
                    HasChildCareReferral = item.HasChildCareReferral,
                    HasPetCareReferral = item.HasPetCareReferral,
                    SelfRegisteredDate = item.SelfRegisteredDate,
                    RegistrationCompletionDate = item.RegistrationCompletionDate,
                    HeadOfHousehold = item.HeadOfHousehold.ToModel(),

                    InterviewerFirstName = item.InterviewerFirstName,
                    InterviewerLastNameInitial = item.InterviewerLastNameInitial,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,

                    //specialNeeds: {};
                    
                };

                if (item.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<Sqlite.Models.FamilyMember>();
                    foreach (var familyMember in item.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToModel());
                    }
                }
            }


            return result;
        }

        public static ViewModels.RegionalDistrict ToViewModel(this Sqlite.Models.RegionalDistrict item)
        {
            ViewModels.RegionalDistrict result = null;
            if (item != null)
            {
                result = new ViewModels.RegionalDistrict()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active
                };
            }
            return result;
        }

    }
}
