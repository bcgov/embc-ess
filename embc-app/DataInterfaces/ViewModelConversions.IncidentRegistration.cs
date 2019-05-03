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

        public static Models.Db.IncidentRegistration ToModel(this ViewModels.Registration source)
        {
            Models.Db.IncidentRegistration result = null;
            if (source != null)
            {
                result = new Models.Db.IncidentRegistration()
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
                    CompletedById = source.CompletedBy == null ? (Guid?)null : Guid.Parse(source.CompletedBy.Externaluseridentifier),
                    HostCommunityId = source.HostCommunity == null ? (Guid?)null : Guid.Parse(source.HostCommunity.Id),
                    IncidentTaskId = source.IncidentTask == null ? (Guid?)null : Guid.Parse(source.IncidentTask.Id),
                };

                result.Evacuees.Add(source.HeadOfHousehold.ToModel());
                foreach (var familyMember in source.HeadOfHousehold.FamilyMembers)
                {
                    var evacuee = familyMember.ToModel();
                    evacuee.EvacueeSequenceNumber = result.Evacuees.Count() + 1;
                    result.Evacuees.Add(evacuee);
                }

                result.IncidentRegistrationAddresses.Add(source.HeadOfHousehold.PrimaryResidence.ToModel(Models.Db.Enumerations.AddressType.Primary));
                if (source.HeadOfHousehold.MailingAddress != null)
                {
                    result.IncidentRegistrationAddresses.Add(source.HeadOfHousehold.MailingAddress.ToModel(Models.Db.Enumerations.AddressType.Mailing));
                }

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
    }
}
