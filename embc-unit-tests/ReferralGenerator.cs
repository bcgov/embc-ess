using Gov.Jag.Embc.Public.Models.Db;
using System;

namespace embc_unit_tests
{
    public class ReferralGenerator
    {
        private static Func<Gov.Jag.Embc.Public.ViewModels.Referral> createBaseReferral = () => new Gov.Jag.Embc.Public.ViewModels.Referral
        {
            Active = true,
            TotalAmount = 124m,
            Comments = "comments",
            Purchaser = "purchaser name",
            Evacuees = new[]
           {
                    new Gov.Jag.Embc.Public.ViewModels.ReferralEvacuee { Id="1" },
                    new Gov.Jag.Embc.Public.ViewModels.ReferralEvacuee { Id="2" }
            },
            ConfirmChecked = true,
            Supplier = new Gov.Jag.Embc.Public.ViewModels.Supplier
            {
                Active = true,
                Name = "supplier1",
                Address = "test",
                City = "city",
                Province = "prov",
                PostalCode = "12334",
                Fax = "112325",
                Telephone = "54356"
            },
            ValidFrom = DateTime.Parse("2019-01-01T11:00"),
            ValidTo = DateTime.Parse("2019-01-05T11:00")
        };

        public static Gov.Jag.Embc.Public.ViewModels.Referral Generate(ReferralType type, string registrationId = "100001")
        {
            var referral = createBaseReferral();
            referral.RegistrationId = registrationId;
            switch (type)
            {
                case ReferralType.Food_Groceries:
                    referral.Type = "Food";
                    referral.SubType = "Groceries";
                    referral.NumDaysMeals = 3;
                    break;

                case ReferralType.Food_Restaurant:
                    referral.Type = "Food";
                    referral.SubType = "Restaurant";
                    referral.NumBreakfasts = 4;
                    referral.NumLunches = 5;
                    referral.NumDinners = 6;
                    break;

                case ReferralType.Transportation_Taxi:
                    referral.Type = "Transportation";
                    referral.SubType = "Taxi";
                    referral.FromAddress = "here";
                    referral.ToAddress = "there";
                    break;

                case ReferralType.Transportation_Other:
                    referral.Type = "Transportation";
                    referral.SubType = "Other";
                    referral.OtherTransportModeDetails = "skytrain and seabus";
                    break;

                case ReferralType.Lodging_Hotel:
                    referral.Type = "Lodging";
                    referral.SubType = "Billeting";
                    referral.NumNights = 5;
                    referral.NumRooms = 2;
                    break;

                case ReferralType.Lodging_Group:
                    referral.Type = "Lodging";
                    referral.SubType = "Group";
                    referral.NumNights = 5;
                    break;

                case ReferralType.Lodging_Billeting:
                    referral.Type = "Lodging";
                    referral.SubType = "Billeting";
                    referral.NumNights = 5;
                    break;

                case ReferralType.Incidentals:
                    referral.Type = "Incidentals";
                    referral.SubType = null;
                    referral.ApprovedItems = "approved items";
                    break;

                case ReferralType.Clothing:
                    referral.Type = "Clothing";
                    referral.SubType = null;
                    referral.ExtremeWinterConditions = true;
                    break;
            }

            return referral;
        }

        public static Gov.Jag.Embc.Public.ViewModels.Referral GenerateWithExcessiveProperties()
        {
            var referral = Generate(ReferralType.Transportation_Other);
            referral.FromAddress = "from";
            referral.ToAddress = "to";

            return referral;
        }
    }
}