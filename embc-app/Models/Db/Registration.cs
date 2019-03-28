using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Evacuee Registration Database Model
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// A system-generated unique identifier
        /// </summary>
        /// <value>A system-generated unique identifier for a Registration</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool Active { get; set; }

        // important
        public bool? RestrictedAccess { get; set; }

        public bool? DeclarationAndConsent { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long EssFileNumber { get; set; }

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

        // requirements (needs assessment)
        public bool? RequiresAccommodation { get; set; }

        public bool? RequiresClothing { get; set; }
        public bool? RequiresFood { get; set; }
        public bool? RequiresIncidentals { get; set; }
        public bool? RequiresTransportation { get; set; }
        public bool? RequiresSupport { get; set; }

        // related entities
        public Guid? HeadOfHouseholdId { get; set; }

        public HeadOfHousehold HeadOfHousehold { get; set; }

        public Guid? IncidentTaskId { get; set; }

        public IncidentTask IncidentTask { get; set; }

        public Guid? HostCommunityId { get; set; }

        public Community HostCommunity { get; set; }

        public Guid? CompletedById { get; set; }

        public Volunteer CompletedBy { get; set; }
    }
}
