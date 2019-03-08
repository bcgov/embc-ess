using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{    

    public class Registration
    {
        public string Id { get; set; }

        public long? EssFileNumber { get; set; }
        public bool? IsRestrictedAccess { get; set; }
        public bool? IsRegisteringFamilyMembers { get; set; }
        public bool? HasDietaryNeeds { get; set; }
        public string DietaryNeedsDetails { get; set; }
        public bool? IsTakingMedication { get; set; }
        public bool? HasThreeDaySupply { get; set; }
        public bool? HasPets { get; set; }
        public string InsuranceCode { get; set; }
        public bool? IsSupportRequired { get; set; }
        public bool? RequiresFood { get; set; }
        public bool? RequiresClothing { get; set; }
        public bool? RequiresAccommodation { get; set; }
        public bool? RequiresIncidentals { get; set; }
        public bool? RequiresTransportation { get; set; }
        public string Facility { get; set; }
        public string DisasterAffectDetails { get; set; }
        public string ExternalReferralsDetails { get; set; }
        public string FamilyRecoveryPlan { get; set; }
        public string FollowUpDetails { get; set; }
        public bool? HasInquiryReferral { get; set; }
        public bool? HasHealthServicesReferral { get; set; }
        public bool? HasFirstAidReferral { get; set; }
        public bool? HasPersonalServicesReferral { get; set; }
        public bool? HasChildCareReferral { get; set; }
        public bool? HasPetCareReferral { get; set; }
        public DateTimeOffset? SelfRegisteredDate { get; set; }
        public DateTimeOffset? RegistrationCompletionDate { get; set; }
        public HeadOfHousehold HeadOfHousehold { get; set; }

        public List<FamilyMember> FamilyMembers { get; set; }

        public User Interviewer { get; set; }
        public string InterviewerFirstName { get; set; }
        public string InterviewerLastNameInitial { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
