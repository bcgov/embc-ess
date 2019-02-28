using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Sqlite.Models
{
    /// <summary>
    /// Role Database Model
    /// </summary>
        public sealed partial class Registration
    {

        public Registration()
        { }

        /// <summary>
        /// A system-generated unique identifier 
        /// </summary>
        /// <value>A system-generated unique identifier for a Role</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public bool? IsRestricted { get; set; }
        public Person FamilyRepresentative { get; set; }
        public bool? IsRegisteringFamilyMembers { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }

        public BceidUser Interviewer { get; set; }
        public string InterviewerFirstName { get; set; }
        public string InterviewerLastNameInitial { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        //specialNeeds: {};

        public bool? IsSupportRequired{ get; set; }

        //public requestedSupportServices{ get; set; }

    }
}
