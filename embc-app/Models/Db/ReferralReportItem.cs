using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Models.Db
{
    [Table("ReferralReportItem")]
    public class ReferralReportItem
    {
        public long Ess_File_Number { get; set; }
        public string Task_Number { get; set; }
        public DateTimeOffset? Task_Number_Start_Date { get; set; }
        public DateTimeOffset? Task_Number_End_Date { get; set; }
        public string File_Status { get; set; }
        public string Evacuated_To { get; set; }
        public string Evacuated_From { get; set; }
        public string Facility_Name { get; set; }
        public string Person_responsible_for_purchasing_goods { get; set; }
        public long Referral_Number { get; set; }
        public string Support_Type { get; set; }
        public string Support_Type2 { get; set; }
        public string Sub_Support_Type { get; set; }
        public string Sub_Support_Type2 { get; set; }
        public DateTime? Valid_From_Date { get; set; }
        public TimeSpan? Valid_From_Time { get; set; }
        public int? Number_Of_Days { get; set; }
        public DateTime? Valid_To_Date { get; set; }
        public TimeSpan? Valid_To_Time { get; set; }
        public int? Number_Of_Evacuees_for_Referral { get; set; }
        public decimal Total_Amount { get; set; }
        public int Breakfasts_per_Person { get; set; }
        public int Lunches_per_Person { get; set; }
        public int Dinners_per_Person { get; set; }
        public int Number_of_Rooms { get; set; }
        public int Number_of_Nights { get; set; }
        public string Mode_of_Transportation { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Address { get; set; }
        public string City { get; set; }
        public string Postal_Code { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }


    }
}
