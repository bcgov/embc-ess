using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Registration ToViewModel(this Models.Db.Registration source)
        {
            ViewModels.Registration result = null;
            if (source != null)
            {
                result = new ViewModels.Registration()
                {
                    Id = source.Id.ToString(),
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
                    FollowUpDetails = source.FollowUpDetails,
                    HasInquiryReferral = source.HasInquiryReferral,
                    HasHealthServicesReferral = source.HasHealthServicesReferral,
                    HasFirstAidReferral = source.HasFirstAidReferral,
                    HasPersonalServicesReferral = source.HasPersonalServicesReferral,
                    HasChildCareReferral = source.HasChildCareReferral,
                    HasPetCareReferral = source.HasPetCareReferral,
                    SelfRegisteredDate = source.SelfRegisteredDate,
                    RegistrationCompletionDate = source.RegistrationCompletionDate,
                    HeadOfHousehold = source.HeadOfHousehold.ToViewModel() as ViewModels.HeadOfHousehold,
                    HostCommunity = source.HostCommunity?.ToViewModel(),
                    CompletedBy = source.CompletedBy == null ? null : source.CompletedBy.ToViewModel(),
                    IncidentTask = source.IncidentTask?.ToViewModel(),
                    DeclarationAndConsent = source.DeclarationAndConsent
                };
            }

            return result;
        }

        public static Models.Db.Registration ToModel(this ViewModels.Registration source)
        {
            Models.Db.Registration result = null;
            if (source != null)
            {
                result = new Models.Db.Registration()
                {
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
                    DeclarationAndConsent = source.DeclarationAndConsent,
                    HeadOfHouseholdId = source.HeadOfHousehold.Id == null ? (Guid?)null : Guid.Parse(source.HeadOfHousehold.Id),
                    HeadOfHousehold = source.HeadOfHousehold.ToModel() as Models.Db.HeadOfHousehold,
                    CompletedById = source.CompletedBy == null ? (int?)null : Convert.ToInt32(source.CompletedBy.Id),
                    HostCommunityId = source.HostCommunity == null ? (Guid?)null : Guid.Parse(source.HostCommunity.Id),
                    IncidentTaskId = source.IncidentTask == null ? (Guid?)null : Guid.Parse(source.IncidentTask.Id),
                };
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }
                if (source.Active.HasValue)
                {
                    result.Active = source.Active.Value;
                }
            }

            return result;
        }

        public static ViewModels.RegistrationSummary ToSummaryViewModel(this Models.Db.Registration source)
        {
            ViewModels.RegistrationSummary result = null;
            if (source != null)
            {
                result = new ViewModels.RegistrationSummary()
                {
                    Id = source.Id.ToString(),
                    EssFileNumber = source.EssFileNumber,
                    RestrictedAccess = source.RestrictedAccess,
                    RegisteringFamilyMembers = source.RegisteringFamilyMembers,
                    RequiresSupport = source.RequiresSupport,
                    RequiresFood = source.RequiresFood,
                    RequiresClothing = source.RequiresClothing,
                    RequiresAccommodation = source.RequiresAccommodation,
                    RequiresIncidentals = source.RequiresIncidentals,
                    RequiresTransportation = source.RequiresTransportation,
                    SelfRegisteredDate = source.SelfRegisteredDate,
                    RegistrationCompletionDate = source.RegistrationCompletionDate,
                    HeadOfHousehold = source.HeadOfHousehold?.ToViewModel() as ViewModels.HeadOfHousehold,
                    IncidentTask = source.IncidentTask?.ToViewModel(),
                    HostCommunity = source.HostCommunity?.ToViewModel()
                };
            }

            return result;
        }
    }
}
