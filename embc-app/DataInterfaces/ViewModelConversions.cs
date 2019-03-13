using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ViewModelConversions
    {
        public static ViewModels.Address ToViewModel(this Sqlite.Models.Address source)
        {
            ViewModels.Address result = null;
            if (source != null)
            {
                result = ViewModels.Address.Create(source.AddressSubtypeCode);
                result.Id = source.Id.ToString();
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.CommunityOrCity = source.CommunityOrCity;
                result.ProvinceOrState = source.Province;
                result.PostalCodeOrZip = source.PostalCode;
                result.Country = source.Country;
            }
            return result;
        }

        public static Sqlite.Models.Address ToModel(this ViewModels.Address source)
        {
            Sqlite.Models.Address result = null;
            if (source != null)
            {
                result = Sqlite.Models.Address.Create(source.AddressSubtypeCode);
                result.AddressLine1 = source.AddressLine1;
                result.AddressLine2 = source.AddressLine2;
                result.AddressLine3 = source.AddressLine3;
                result.CommunityOrCity = source.CommunityOrCity;
                result.Province = source.ProvinceOrState;
                result.PostalCode = source.PostalCodeOrZip;
                result.Country = source.Country;
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                // TODO: Add fields for each subtype
                if (result is Sqlite.Models.BcAddress)
                {
                    // TODO: ...
                }
                if (result is Sqlite.Models.OtherAddress)
                {
                    // TODO: ...
                }
            }
            return result;
        }

        public static ViewModels.Community ToViewModel(this Sqlite.Models.Community source)
        {
            ViewModels.Community result = null;
            if (source != null)
            {
                result = new ViewModels.Community()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active,
                    RegionalDistrict = source.RegionalDistrict.ToViewModel()
                };
            }
            return result;
        }

        public static ViewModels.Country ToViewModel(this Sqlite.Models.Country source)
        {
            ViewModels.Country result = null;
            if (source != null)
            {
                result = new ViewModels.Country()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }

        public static Sqlite.Models.Person ToModel(this ViewModels.Person source)
        {
            Sqlite.Models.Person result = null;
            if (source != null)
            {
                result = Sqlite.Models.Person.Create(source.PersonType);
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Nickname = source.Nickname;
                result.Initials = source.Initials;
                result.Gender = source.Gender;
                result.Dob = source.Dob;
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }

                // TODO: Add fields for HOH, FMBR, VOLN
                if (result is Sqlite.Models.HeadOfHousehold)
                {
                    //...
                }
                if (result is Sqlite.Models.FamilyMember)
                {
                    //...
                }
                if (result is Sqlite.Models.Volunteer)
                {
                    //...
                }
            }
            return result;
        }

        public static ViewModels.Person ToViewModel(this Sqlite.Models.Person source)
        {
            ViewModels.Person result = null;
            if (source != null)
            {
                result = ViewModels.Person.Create(source.PersonType);
                result.Id = source.Id.ToString();
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Nickname = source.Nickname;
                result.Initials = source.Initials;
                result.Gender = source.Gender;
                result.Dob = source.Dob;

                // TODO: Add fields for HOH, FMBR, VOLN
                if (result is ViewModels.HeadOfHousehold)
                {
                    //...
                }
                if (result is ViewModels.FamilyMember)
                {
                    //...
                }
                if (result is ViewModels.Volunteer)
                {
                    //...
                }
            }
            return result;
        }

        public static Sqlite.Models.FamilyMember ToModel(this ViewModels.FamilyMember source)
        {
            Sqlite.Models.FamilyMember result = null;
            if (source != null)
            {
                result = new Sqlite.Models.FamilyMember()
                {
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Nickname = source.Nickname,
                    Initials = source.Initials,
                    Gender = source.Gender,
                    Dob = source.Dob,
                    RelationshipToEvacuee = source.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = source.SameLastNameAsEvacuee,
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
            }
            return result;
        }

        public static ViewModels.FamilyMember ToViewModel(this Sqlite.Models.FamilyMember source)
        {
            ViewModels.FamilyMember result = null;
            if (source != null)
            {
                result = new ViewModels.FamilyMember()
                {
                    Id = source.Id.ToString(),
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Nickname = source.Nickname,
                    Initials = source.Initials,
                    Gender = source.Gender,
                    Dob = source.Dob,
                    RelationshipToEvacuee = source.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = source.SameLastNameAsEvacuee,
                };
            }
            return result;
        }

        public static ViewModels.HeadOfHousehold ToViewModel(this Sqlite.Models.HeadOfHousehold source)
        {
            ViewModels.HeadOfHousehold result = null;
            if (source != null)
            {
                result = new ViewModels.HeadOfHousehold()
                {
                    Id = source.Id.ToString(),
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Nickname = source.Nickname,
                    Initials = source.Initials,
                    Gender = source.Gender,
                    Dob = source.Dob,
                    PhoneNumber = source.PhoneNumber,
                    PhoneNumberAlt = source.PhoneNumberAlt,
                    Email = source.Email,

                    PrimaryResidence = source.PrimaryResidence.ToViewModel(),
                    MailingAddress = source.MailingAddress.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.HeadOfHousehold ToModel(this ViewModels.HeadOfHousehold source)
        {
            Sqlite.Models.HeadOfHousehold result = null;
            if (source != null)
            {
                result = new Sqlite.Models.HeadOfHousehold()
                {
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Nickname = source.Nickname,
                    Initials = source.Initials,
                    Gender = source.Gender,
                    Dob = source.Dob,
                    PhoneNumber = source.PhoneNumber,
                    PhoneNumberAlt = source.PhoneNumberAlt,
                    Email = source.Email,

                    PrimaryResidence = source.PrimaryResidence.ToModel(),
                    MailingAddress = source.MailingAddress.ToModel()
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
            }
            return result;
        }

        public static ViewModels.Region ToViewModel(this Sqlite.Models.Region source)
        {
            ViewModels.Region result = null;
            if (source != null)
            {
                result = new ViewModels.Region()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }

        public static ViewModels.Registration ToViewModel(this Sqlite.Models.Registration source)
        {
            ViewModels.Registration result = null;
            if (source != null)
            {
                result = new ViewModels.Registration()
                {
                    Id = source.Id.ToString(),
                    EssFileNumber = source.EssFileNumber,
                    RestrictedAccess = source.RestrictedAccess,
                    RegisteringFamilyMembers = source.RegisteringFamilyMembers,
                    DietaryNeeds = source.DietaryNeeds,
                    DietaryNeedsDetails = source.DietaryNeedsDetails,
                    MedicationNeeds = source.MedicationNeeds,
                    HasThreeDayMedicationSupply = source.HasThreeDayMedicationSupply,
                    HasPets = source.HasPets,
                    InsuranceCode = source.InsuranceCode,
                    RequiresSupport = source.RequiresSupport,
                    RequiresFood = source.RequiresFood,

                    RequiresClothing = source.RequiresClothing,
                    RequiresAccommodation = source.RequiresAccommodation,
                    RequiresIncidentals = source.RequiresIncidentals,
                    RequiresTransportation = source.RequiresTransportation,
                    Facility = source.Facility,
                    DisasterAffectDetails = source.DisasterAffectDetails,
                    ExternalReferralsDetails = source.ExternalReferralsDetails,
                    FamilyRecoveryPlan = source.FamilyRecoveryPlan,
                    FollowUpDetails = source.FollowUpDetails,
                    HasInquiryReferral = source.HasInquiryReferral,
                    HasHealthServicesReferral = source.HasHealthServicesReferral,
                    HasFirstAidReferral = source.HasFirstAidReferral,
                    HasPersonalServicesReferral = source.HasPersonalServicesReferral,
                    HasChildCareReferral = source.HasChildCareReferral,
                    HasPetCareReferral = source.HasPetCareReferral,
                    SelfRegisteredDate = source.SelfRegisteredDate,
                    RegistrationCompletionDate = source.RegistrationCompletionDate,
                    HeadOfHousehold = source.HeadOfHousehold.ToViewModel(),

                };

                if (source.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<ViewModels.FamilyMember>();
                    foreach (var familyMember in source.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToViewModel());
                    }
                }
            }

            return result;
        }

        public static Sqlite.Models.Registration ToModel(this ViewModels.Registration source)
        {
            Sqlite.Models.Registration result = null;
            if (source != null)
            {
                result = new Sqlite.Models.Registration()
                {
                    EssFileNumber = source.EssFileNumber,
                    RestrictedAccess = source.RestrictedAccess,
                    RegisteringFamilyMembers = source.RegisteringFamilyMembers,
                    DietaryNeeds = source.DietaryNeeds,
                    DietaryNeedsDetails = source.DietaryNeedsDetails,
                    MedicationNeeds = source.MedicationNeeds,
                    HasThreeDayMedicationSupply = source.HasThreeDayMedicationSupply,
                    HasPets = source.HasPets,
                    InsuranceCode = source.InsuranceCode,
                    RequiresSupport = source.RequiresSupport,
                    RequiresFood = source.RequiresFood,

                    RequiresClothing = source.RequiresClothing,
                    RequiresAccommodation = source.RequiresAccommodation,
                    RequiresIncidentals = source.RequiresIncidentals,
                    RequiresTransportation = source.RequiresTransportation,
                    Facility = source.Facility,
                    DisasterAffectDetails = source.DisasterAffectDetails,
                    ExternalReferralsDetails = source.ExternalReferralsDetails,
                    FamilyRecoveryPlan = source.FamilyRecoveryPlan,
                    FollowUpDetails = source.FollowUpDetails,
                    HasInquiryReferral = source.HasInquiryReferral,
                    HasHealthServicesReferral = source.HasHealthServicesReferral,
                    HasFirstAidReferral = source.HasFirstAidReferral,
                    HasPersonalServicesReferral = source.HasPersonalServicesReferral,
                    HasChildCareReferral = source.HasChildCareReferral,
                    HasPetCareReferral = source.HasPetCareReferral,
                    SelfRegisteredDate = source.SelfRegisteredDate,
                    RegistrationCompletionDate = source.RegistrationCompletionDate,
                    HeadOfHousehold = source.HeadOfHousehold.ToModel(),
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }

                if (source.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<Sqlite.Models.FamilyMember>();
                    foreach (var familyMember in source.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToModel());
                    }
                }
            }

            return result;
        }

        public static ViewModels.RegionalDistrict ToViewModel(this Sqlite.Models.RegionalDistrict source)
        {
            ViewModels.RegionalDistrict result = null;
            if (source != null)
            {
                result = new ViewModels.RegionalDistrict()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Active = source.Active
                };
            }
            return result;
        }

    }
}
