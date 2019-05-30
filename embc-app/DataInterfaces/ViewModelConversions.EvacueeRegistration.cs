using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Registration ToViewModel(this Models.Db.EvacueeRegistration source)
        {
            var result = new ViewModels.Registration
            {
                Id = source.EssFileNumber.ToString(),
                Active = source.Active,
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
                InternalCaseNotes = source.FollowUpDetails,
                HasInquiryReferral = source.HasInquiryReferral,
                HasHealthServicesReferral = source.HasHealthServicesReferral,
                HasFirstAidReferral = source.HasFirstAidReferral,
                HasPersonalServicesReferral = source.HasPersonalServicesReferral,
                HasChildCareReferral = source.HasChildCareReferral,
                HasPetCareReferral = source.HasPetCareReferral,
                SelfRegisteredDate = source.SelfRegisteredDate,
                RegistrationCompletionDate = source.RegistrationCompletionDate,
                HeadOfHousehold = source.Evacuees.Single(e => e.EvacueeType == EvacueeType.HeadOfHousehold).ToViewModel(source) as ViewModels.HeadOfHousehold,
                HostCommunity = mapper.Map<ViewModels.Community>(source.HostCommunity),
                IncidentTask = source.IncidentTask?.ToViewModel(),
                DeclarationAndConsent = source.DeclarationAndConsent,
                IsFinalized = source.IsFinalized
            };

            return result;
        }

        public static ViewModels.RegistrationSummary ToSummaryViewModel(this Models.Db.EvacueeRegistration source)
        {
            var fullViewModel = source.ToViewModel();
            var result = new ViewModels.RegistrationSummary()
            {
                Id = fullViewModel.Id,
                EssFileNumber = fullViewModel.EssFileNumber,
                RestrictedAccess = fullViewModel.RestrictedAccess,
                RegisteringFamilyMembers = fullViewModel.RegisteringFamilyMembers,
                RequiresSupport = fullViewModel.RequiresSupport,
                RequiresFood = fullViewModel.RequiresFood,
                RequiresClothing = fullViewModel.RequiresClothing,
                RequiresAccommodation = fullViewModel.RequiresAccommodation,
                RequiresIncidentals = fullViewModel.RequiresIncidentals,
                RequiresTransportation = fullViewModel.RequiresTransportation,
                SelfRegisteredDate = fullViewModel.SelfRegisteredDate,
                RegistrationCompletionDate = fullViewModel.RegistrationCompletionDate,
                HeadOfHousehold = fullViewModel.HeadOfHousehold,
                IncidentTask = fullViewModel.IncidentTask,
                HostCommunity = fullViewModel.HostCommunity,
                Active = fullViewModel.Active ?? false,
                HasInternalCaseNotes = !string.IsNullOrWhiteSpace(fullViewModel.InternalCaseNotes),
                Facility = fullViewModel.Facility
            };

            return result;
        }

        public static Models.Db.EvacueeRegistration ToModel(this ViewModels.Registration source)
        {
            var result = new Models.Db.EvacueeRegistration
            {
                RestrictedAccess = source.RestrictedAccess,

                PhoneNumber = source.HeadOfHousehold.PhoneNumber,
                PhoneNumberAlt = source.HeadOfHousehold.PhoneNumberAlt,
                Email = source.HeadOfHousehold.Email,

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
                FollowUpDetails = source.InternalCaseNotes,
                HasInquiryReferral = source.HasInquiryReferral,
                HasHealthServicesReferral = source.HasHealthServicesReferral,
                HasFirstAidReferral = source.HasFirstAidReferral,
                HasPersonalServicesReferral = source.HasPersonalServicesReferral,
                HasChildCareReferral = source.HasChildCareReferral,
                HasPetCareReferral = source.HasPetCareReferral,
                SelfRegisteredDate = source.SelfRegisteredDate,
                RegistrationCompletionDate = source.RegistrationCompletionDate,
                DeclarationAndConsent = source.DeclarationAndConsent,
                CompletedById = source.CompletedBy?.Externaluseridentifier,
                HostCommunityId = source.HostCommunity == null ? (Guid?)null : Guid.Parse(source.HostCommunity.Id),
                IncidentTaskId = source.IncidentTask == null ? (Guid?)null : Guid.Parse(source.IncidentTask.Id),
            };

            result.Evacuees.Add(source.HeadOfHousehold.ToModel());
            foreach (var familyMember in source.HeadOfHousehold.FamilyMembers)
            {
                var evacuee = familyMember.ToModel();
                if (string.IsNullOrEmpty(familyMember.Id))
                {
                    evacuee.EvacueeSequenceNumber = result.Evacuees.Count() + 1;
                }
                result.Evacuees.Add(evacuee);
            }

            result.EvacueeRegistrationAddresses.Add(source.HeadOfHousehold.PrimaryResidence.ToModel(Models.Db.Enumerations.AddressType.Primary));
            if (source.HeadOfHousehold.MailingAddress != null)
            {
                result.EvacueeRegistrationAddresses.Add(source.HeadOfHousehold.MailingAddress.ToModel(Models.Db.Enumerations.AddressType.Mailing));
            }

            if (source.Id != null)
            {
                result.EssFileNumber = long.Parse(source.Id);
            }
            if (source.Active.HasValue)
            {
                result.Active = source.Active.Value;
            }

            return result;
        }

        public static ViewModels.Person ToViewModel(this Models.Db.Evacuee source, Models.Db.EvacueeRegistration evacueeRegistration)
        {
            ViewModels.Person result;
            var isHeadOfHousehold = false;
            if (source.EvacueeTypeCode == EvacueeType.HeadOfHousehold.GetDisplayName())
            {
                result = new ViewModels.HeadOfHousehold();
                isHeadOfHousehold = true;
            }
            else
            {
                result = new ViewModels.FamilyMember();
            }

            result.Id = source.RegistrationIdSeq;

            result.FirstName = source.FirstName;
            result.LastName = source.LastName;

            if (isHeadOfHousehold)
            {
                var resultHoh = result as ViewModels.HeadOfHousehold;
                resultHoh.PhoneNumber = evacueeRegistration.PhoneNumber;
                resultHoh.PhoneNumberAlt = evacueeRegistration.PhoneNumberAlt;
                resultHoh.Email = evacueeRegistration.Email;

                resultHoh.PrimaryResidence = evacueeRegistration.EvacueeRegistrationAddresses.Single(a => a.AddressType == AddressType.Primary).ToViewModel();
                if (evacueeRegistration.EvacueeRegistrationAddresses.Any(a => a.AddressType == AddressType.Mailing))
                {
                    resultHoh.MailingAddress = evacueeRegistration.EvacueeRegistrationAddresses.Single(a => a.AddressType == AddressType.Mailing).ToViewModel();
                }

                var familyMembers = evacueeRegistration.Evacuees.Where(e => e.EvacueeTypeCode != EvacueeType.HeadOfHousehold.GetDisplayName());
                if (familyMembers.Any())
                {
                    resultHoh.FamilyMembers = new List<ViewModels.FamilyMember>();
                    foreach (var familyMember in familyMembers)
                    {
                        resultHoh.FamilyMembers.Add(familyMember.ToViewModel(evacueeRegistration) as ViewModels.FamilyMember);
                    }
                }
            }

            var resultEvacuee = result as ViewModels.Evacuee;
            resultEvacuee.Nickname = source.Nickname;
            resultEvacuee.Initials = source.Initials;
            resultEvacuee.Gender = source.Gender;
            resultEvacuee.Dob = source.Dob;

            if (!isHeadOfHousehold)
            {
                var resultFm = result as ViewModels.FamilyMember;
                resultFm.RelationshipToEvacuee = mapper.Map<ViewModels.FamilyRelationshipType>(source.EvacueeTypeCode == EvacueeType.ImmediateFamily.GetDisplayName()
                    ? EvacueeType.ImmediateFamily
                    : EvacueeType.ExtendedFamily);
                resultFm.SameLastNameAsEvacuee = source.SameLastNameAsEvacuee;
            }
            return result;
        }

        public static Models.Db.Evacuee ToModel(this ViewModels.Person source)
        {
            var result = new Models.Db.Evacuee();

            if (source.Id != null)
            {
                result.RegistrationId = Models.Db.Evacuee.GetEvacueeRegistrationIdFromIncidentRegSeqId(source.Id);
                result.EvacueeSequenceNumber = Models.Db.Evacuee.GetEvacueeSequenceNumberFromIncidentRegSeqId(source.Id);
            }

            result.FirstName = source.FirstName;
            result.LastName = source.LastName;

            if (source is ViewModels.Evacuee sourceEvacuee)
            {
                result.Nickname = sourceEvacuee.Nickname;
                result.Initials = sourceEvacuee.Initials;
                result.Gender = source.Gender;
                result.Dob = sourceEvacuee.Dob;
            }

            if (source is ViewModels.HeadOfHousehold sourceHoh)
            {
                result.EvacueeSequenceNumber = 1;
                result.EvacueeTypeCode = EvacueeType.HeadOfHousehold.GetDisplayName();
            }

            if (source is ViewModels.FamilyMember sourceFm)
            {
                result.EvacueeTypeCode = sourceFm.RelationshipToEvacuee.Code;
                result.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
            }

            return result;
        }

        public static ViewModels.Address ToViewModel(this Models.Db.EvacueeRegistrationAddress source)
        {
            var result = new ViewModels.Address
            {
                Id = source.RegistrationId.ToString(),
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                PostalCode = source.PostalCode,
                Province = source.Province,
                Country = mapper.Map<ViewModels.Country>(source.Country),
                AddressSubtype = source.AddressSubtypeCode,
                Community = mapper.Map<ViewModels.Community>(source.Community),
                City = source.City
            };
            return result;
        }

        public static Models.Db.EvacueeRegistrationAddress ToModel(this ViewModels.Address source, AddressType addressType)
        {
            var result = new Models.Db.EvacueeRegistrationAddress
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                PostalCode = source.PostalCode,
                Province = source.Province,
                CountryCode = source.Country.CountryCode
            };

            if (source.Id != null)
            {
                result.RegistrationId = long.Parse(source.Id);
            }
            result.AddressSequenceNumber = addressType == AddressType.Primary ? 1 : 2;

            result.AddressTypeCode = addressType.GetDisplayName();

            if (source.isBcAddress)
            {
                result.AddressSubtypeCode = AddressSubType.BCAddress.GetDisplayName();
                result.CommunityId = Guid.Parse(source.Community.Id);
            }
            if (source.isOtherAddress)
            {
                result.AddressSubtypeCode = AddressSubType.OtherAddress.GetDisplayName();
                result.City = source.City;
            }
            return result;
        }
    }
}
