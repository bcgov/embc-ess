using System;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class Registration
    {
        public string Id { get; set; }

        // important
        public bool? RestrictedAccess { get; set; }

        public bool? DeclarationAndConsent { get; set; }
        public long? EssFileNumber { get; set; }

        // registration record
        public bool? DietaryNeeds { get; set; }

        public string DietaryNeedsDetails { get; set; }
        public string DisasterAffectDetails { get; set; }
        public string ExternalReferralsDetails { get; set; }
        public string Facility { get; set; }
        public string FamilyRecoveryPlan { get; set; }
        public string FollowUpDetails { get; set; }
        public string InsuranceCode { get; set; }  // one of ['yes', 'yes-unsure', 'no', 'unsure']
        public bool? MedicationNeeds { get; set; }
        public DateTimeOffset? SelfRegisteredDate { get; set; }
        public DateTimeOffset? RegistrationCompletionDate { get; set; }
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
        // public Community HostCommunity { get; set; }

        // TODO: Should we link to the full User record for an interviewer or just capture basic info (name + last name initial)?
        // public Volunteer CompletedBy { get; set; }
    }
}
