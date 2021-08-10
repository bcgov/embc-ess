using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    [Table("EvacueeReportItem")]
    public class EvacueeReportItem
    {
        public long Ess_File_Number { get; set; }
        public string Task_Number { get; set; }
        public DateTime? Task_Number_Start_Date { get; set; }
        public DateTime? Task_Number_End_Date { get; set; }
        public string File_Status { get; set; }
        public string Evacuated_To { get; set; }
        public string Evacuated_From { get; set; }
        public string Facility_Name { get; set; }
        public DateTime? Self_Registration_Date { get; set; }
        public DateTime? Registration_Completed_Date { get; set; }
        /*****************************************************************
         * PI columns 
         *****************************************************************/
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Gender { get; set; }
        public string Is_Head_Of_HouseHold { get; set; }
        public string Preferred_Name { get; set; }
        public string Initials { get; set; }
        public string Address { get; set; }
        public string Community { get; set; }
        public string Province { get; set; }
        public string Postal_Code { get; set; }
        public string Country { get; set; }
        public string Registration_Phone_Number { get; set; }
        public string Registration_Alternate_Phone_Number { get; set; }
        public string Registration_Email_Address { get; set; }
        public string Mailing_Address { get; set; }
        public string Mailing_Community { get; set; }
        public string Mailing_Province { get; set; }
        public string Mailing_Postal_Code { get; set; }
        public string Mailing_Country { get; set; }
        /***************************************************************/
        public string Insurance { get; set; }
        public string Pets { get; set; }
        public string Service_Recommendation_Inquiry { get; set; }
        public string Service_Recommendation_Health_Services { get; set; }
        public string Service_Recommendation_First_Aid { get; set; }
        public string Service_Recommendation_Personal_Services { get; set; }
        public string Service_Recommendation_Child_Care { get; set; }
        public string Service_Recommendation_Pet_Care { get; set; }
        // *** NEW***
        public string Requires_Accommodation { get; set; }
        public string Requires_Clothing { get; set; }
        public string Requires_Food { get; set; }
        public string Requires_Incidentals { get; set; }
        public string Requires_Transportation { get; set; }
        public string Medication_Needs { get; set; }
        public string Medication_supply_for_3_days { get; set; }
        public string Dietary_Needs { get; set; }
        /* Jira EMBCESSMOD-2323 potential fields with characters affecting the csv file output
        //public string Pet_Care_Plan { get; set; }
        //public string Dietary_Needs_Details { get; set; }
        //public string Disaster_Affect_Details { get; set; }
        //public string External_Service_Recommendations { get; set; }
        //public string Family_Recovery_Plan { get; set; }
        //public string Internal_Case_Notes { get; set; }
        */
    }
}
