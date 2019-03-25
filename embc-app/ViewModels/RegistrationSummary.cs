using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.ViewModels
{

    public class RegistrationSummary
    {
        public string Id { get; set; }

        // important
        public bool? RestrictedAccess { get; set; }
        public bool? DeclarationAndConsent { get; set; }
        public long? EssFileNumber { get; set; }

        // registration record
        public DateTimeOffset? SelfRegisteredDate { get; set; }
        public DateTimeOffset? RegistrationCompletionDate { get; set; }
        public string RegisteringFamilyMembers { get; set; }  // one of ['yes', 'yes-later', 'no']
        
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
        public Community HostCommunity { get; set; }

    }
}
