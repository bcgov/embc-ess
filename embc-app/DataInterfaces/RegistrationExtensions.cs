using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class RegistrationExtensions
    {
        public static void PatchValues(this Sqlite.Models.Registration self, ViewModels.Registration values)
        {
            // important
            self.RestrictedAccess = values.RestrictedAccess;
            self.DeclarationAndConsent = values.DeclarationAndConsent;
            self.EssFileNumber = values.EssFileNumber;
            // registration record
            self.DietaryNeeds = values.DietaryNeeds;
            self.DietaryNeedsDetails = values.DietaryNeedsDetails;
            self.DisasterAffectDetails = values.DisasterAffectDetails;
            self.ExternalReferralsDetails = values.ExternalReferralsDetails;
            self.Facility = values.Facility;
            self.FamilyRecoveryPlan = values.FamilyRecoveryPlan;
            self.FollowUpDetails = values.FollowUpDetails;
            self.InsuranceCode = values.InsuranceCode;
            self.MedicationNeeds = values.MedicationNeeds;
            self.SelfRegisteredDate = values.SelfRegisteredDate;
            self.RegistrationCompletionDate = values.RegistrationCompletionDate;
            self.RegisteringFamilyMembers = values.RegisteringFamilyMembers;
            // family state flags
            self.HasThreeDayMedicationSupply = values.HasThreeDayMedicationSupply;
            self.HasInquiryReferral = values.HasInquiryReferral;
            self.HasHealthServicesReferral = values.HasHealthServicesReferral;
            self.HasFirstAidReferral = values.HasFirstAidReferral;
            self.HasChildCareReferral = values.HasChildCareReferral;
            self.HasPersonalServicesReferral = values.HasPersonalServicesReferral;
            self.HasPetCareReferral = values.HasPetCareReferral;
            self.HasPets = values.HasPets;
            // requirements (needs assessment)
            self.RequiresAccommodation = values.RequiresAccommodation;
            self.RequiresClothing = values.RequiresClothing;
            self.RequiresFood = values.RequiresFood;
            self.RequiresIncidentals = values.RequiresIncidentals;
            self.RequiresTransportation = values.RequiresTransportation;
            self.RequiresSupport = values.RequiresSupport;
            // related entities
            self.HeadOfHousehold = values.HeadOfHousehold.ToModel() as Sqlite.Models.HeadOfHousehold;

            // TODO: Add these viewmodels in!
            // self.IncidentTask = values.IncidentTask.ToModel();
            // self.HostCommunity = values.HostCommunity.ToModel();
        }

        public static void PatchValues(this Sqlite.Models.Registration self, Sqlite.Models.Registration values)
        {
            // important
            self.RestrictedAccess = values.RestrictedAccess;
            self.DeclarationAndConsent = values.DeclarationAndConsent;
            self.EssFileNumber = values.EssFileNumber;
            // registration record
            self.DietaryNeeds = values.DietaryNeeds;
            self.DietaryNeedsDetails = values.DietaryNeedsDetails;
            self.DisasterAffectDetails = values.DisasterAffectDetails;
            self.ExternalReferralsDetails = values.ExternalReferralsDetails;
            self.Facility = values.Facility;
            self.FamilyRecoveryPlan = values.FamilyRecoveryPlan;
            self.FollowUpDetails = values.FollowUpDetails;
            self.InsuranceCode = values.InsuranceCode;
            self.MedicationNeeds = values.MedicationNeeds;
            self.SelfRegisteredDate = values.SelfRegisteredDate;
            self.RegistrationCompletionDate = values.RegistrationCompletionDate;
            self.RegisteringFamilyMembers = values.RegisteringFamilyMembers;
            // family state flags
            self.HasThreeDayMedicationSupply = values.HasThreeDayMedicationSupply;
            self.HasInquiryReferral = values.HasInquiryReferral;
            self.HasHealthServicesReferral = values.HasHealthServicesReferral;
            self.HasFirstAidReferral = values.HasFirstAidReferral;
            self.HasChildCareReferral = values.HasChildCareReferral;
            self.HasPersonalServicesReferral = values.HasPersonalServicesReferral;
            self.HasPetCareReferral = values.HasPetCareReferral;
            self.HasPets = values.HasPets;
            // requirements (needs assessment)
            self.RequiresAccommodation = values.RequiresAccommodation;
            self.RequiresClothing = values.RequiresClothing;
            self.RequiresFood = values.RequiresFood;
            self.RequiresIncidentals = values.RequiresIncidentals;
            self.RequiresTransportation = values.RequiresTransportation;
            self.RequiresSupport = values.RequiresSupport;
            // related entities
            self.HeadOfHousehold = values.HeadOfHousehold;
            self.IncidentTask = values.IncidentTask;
            self.HostCommunity = values.HostCommunity;
        }
    }
}
