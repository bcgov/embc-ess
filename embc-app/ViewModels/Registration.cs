using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.ViewModels
{    

    public class Registration
    {
        public string Id { get; set; }

        public bool? IsRestricted { get; set; }
        public Person FamilyRepresentative { get; set; }
        public bool? IsRegisteringFamilyMembers { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }

        // public BceidUser Interviewer { get; set; }
        public string InterviewerFirstName { get; set; }
        public string InterviewerLastNameInitial { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        //specialNeeds: {};

        public bool? IsSupportRequired { get; set; }

    }
}
