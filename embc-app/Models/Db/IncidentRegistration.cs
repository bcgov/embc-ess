using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    public class IncidentRegistration
    {
        public IncidentRegistration()
        {
            Evacuees = new List<Evacuee>();
            IncidentRegistrationAddresses = new List<IncidentRegistrationAddress>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public bool? RestrictedAccess { get; set; }

        public bool? DeclarationAndConsent { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long EssFileNumber { get; set; }

        public string PhoneNumber { get; set; }  //TODO:  Moved from HOH
        public string PhoneNumberAlt { get; set; } //TODO:  Moved from HOH
        public string Email { get; set; } //TODO:  Moved from HOH

        public bool? DietaryNeeds { get; set; }
        public string DietaryNeedsDetails { get; set; }
        public string DisasterAffectDetails { get; set; }
        public string ExternalReferralsDetails { get; set; }
        public string Facility { get; set; }
        public string FamilyRecoveryPlan { get; set; }
        public string FollowUpDetails { get; set; }
        public string InsuranceCode { get; set; }
        public bool? MedicationNeeds { get; set; }
        public DateTimeOffset? SelfRegisteredDate { get; set; }
        public DateTimeOffset? RegistrationCompletionDate { get; set; }
        public string RegisteringFamilyMembers { get; set; }  //TODO:  Not on New Model

        public bool? HasThreeDayMedicationSupply { get; set; }
        public bool? HasInquiryReferral { get; set; }
        public bool? HasHealthServicesReferral { get; set; }
        public bool? HasFirstAidReferral { get; set; }
        public bool? HasChildCareReferral { get; set; }
        public bool? HasPersonalServicesReferral { get; set; }
        public bool? HasPetCareReferral { get; set; }
        public bool? HasPets { get; set; }
        public bool? RequiresAccommodation { get; set; }
        public bool? RequiresClothing { get; set; }
        public bool? RequiresFood { get; set; }
        public bool? RequiresIncidentals { get; set; }
        public bool? RequiresTransportation { get; set; }
        public bool? RequiresSupport { get; set; }

        public Guid? IncidentTaskId { get; set; }

        public IncidentTask IncidentTask { get; set; }

        public Guid? HostCommunityId { get; set; }

        public Community HostCommunity { get; set; }

        public Guid? CompletedById { get; set; }

        public List<Evacuee> Evacuees { get; set; }

        public List<IncidentRegistrationAddress> IncidentRegistrationAddresses { get; set; }
    }
}
