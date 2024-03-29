using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gov.Jag.Embc.Public.Models.Db
{
    [Table("ReferralReportItem")]
    public class ReferralReportItem
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
        public string Person_responsible_for_purchasing_goods { get; set; }
        public long Referral_Number { get; set; }
        /*****************************************************************
         * PI columns 
         *****************************************************************/
        public string Support_Type { get; set; }
        public string Sub_Support_Type { get; set; }
        public DateTime? Valid_From_Date { get; set; }
        public int? Number_Of_Days { get; set; }
        public DateTime? Valid_To_Date { get; set; }
        public int? Number_Of_Evacuees_for_Referral { get; set; }
        public decimal? Total_Amount { get; set; }
        public int? Breakfasts_per_Person { get; set; }
        public int? Lunches_per_Person { get; set; }
        public int? Dinners_per_Person { get; set; }
        public int? Number_of_Rooms { get; set; }
        public int? Number_of_Nights { get; set; }
        public DateTime? Referral_Created_Date { get; set; }
        public string Clothing_Extreme_Winter_Conditions { get; set; }
        public int? Groceries_Number_Of_Meals { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Address { get; set; }
        public string Supplier_City { get; set; }
        public string Supplier_Postal_Code { get; set; }
        public string Supplier_Telephone { get; set; }
        public string Supplier_Fax { get; set; }
        /* Jira EMBCESSMOD-2323 potential fields with characters affecting the csv file output
        //public string Referral_Comments { get; set; }
        //public string Mode_of_Transportation { get; set; }
        //public string Taxi_From_Address { get; set; }
        //public string Taxi_To_Address { get; set; }
        //public string Incidentals_Approved_Items { get; set; }
        */
        /**************************************************************/
    }
}
