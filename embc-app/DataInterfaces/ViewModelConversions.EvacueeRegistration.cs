using Gov.Jag.Embc.Public.Utils;
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
                IncidentTask = mapper.Map<ViewModels.IncidentTask>(source.IncidentTask),
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

        private static ViewModels.Evacuee ToViewModel(this Models.Db.Evacuee source, Models.Db.EvacueeRegistration evacueeRegistration)
        {
            ViewModels.Evacuee result;
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

            result.Id = $"{source.RegistrationId.ToString()}-{source.EvacueeSequenceNumber}";

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
                    //foreach (var familyMember in familyMembers)
                    //{
                    //    resultHoh.FamilyMembers.Add(familyMember.ToViewModel(evacueeRegistration) as ViewModels.FamilyMember);
                    //}
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

        private static ViewModels.Address ToViewModel(this Models.Db.EvacueeRegistrationAddress source)
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
    }
}
