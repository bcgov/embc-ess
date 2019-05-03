using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Registration ToViewModel(this Models.Db.IncidentRegistration source)
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
                    //TODO: HeadOfHousehold conversion for Registration ViewModel
                    //HeadOfHousehold = source.HeadOfHousehold.ToViewModel() as ViewModels.HeadOfHousehold,
                    HostCommunity = source.HostCommunity?.ToViewModel(),
                    IncidentTask = source.IncidentTask?.ToViewModel(),
                    DeclarationAndConsent = source.DeclarationAndConsent
                };
            }

            return result;
        }
    }
}
