using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IEnumerable<EvacueeListItem>> GetEvacueesAsync(EvacueeSearchQueryParameters searchQuery)
        {
            var query = AssembleQuery(searchQuery);

            return await query.Sort(MapSortToFields(searchQuery.SortBy)).Distinct().Select(e => mapper.Map<EvacueeListItem>(e)).ToArrayAsync();
        }

        public async Task<IPagedResults<EvacueeListItem>> GetEvacueesPaginatedAsync(EvacueeSearchQueryParameters searchQuery)
        {
            var query = AssembleQuery(searchQuery);

            var pagedQuery = new PaginatedQuery<Models.Db.ViewEvacuee>(query, searchQuery.Offset, searchQuery.Limit);

            var evacuees = await pagedQuery.Query.Sort(MapSortToFields(searchQuery.SortBy)).Distinct().Select(e => mapper.Map<EvacueeListItem>(e)).ToArrayAsync();

            return new PaginatedList<EvacueeListItem>(evacuees, pagedQuery.Pagination);
        }

        private IQueryable<Models.Db.ViewEvacuee> AssembleQuery(EvacueeSearchQueryParameters searchQuery)
        {
            if (searchQuery.HasSortBy())
            {
                // Sort by whatever parameter was included with the query
                searchQuery.SortBy = MapSortToFields(searchQuery.SortBy);
            }
            else
            {
                // default search is always sort descending by ess file number
                searchQuery.SortBy = "-essFileNumber";
            };

            var query = db.ViewEvacuees
                // Inactive evacuees are soft deleted. We do not return them or give the user the
                // option yet.
                .Where(e => e.Active == searchQuery.Active)
                // we sort the larger collection first before letting the subset (paginated ones be sorted)
                .Sort(MapSortToFields(searchQuery.SortBy));

            if (searchQuery.HasQuery())
            {
                // Try to parse the query as a date - if it fails it sets dob to DateTime.MinValue
                // (Midnight @ 0001 AD), which shouldn't match anyone
                DateTime.TryParse(searchQuery.Query, out DateTime dob);

                // Simple search. When a search query is provided search should match multiple
                // things from the record. Query can match multiple things.
                query = query.Where(e =>
                    EF.Functions.Like(e.LastName, $"%{searchQuery.Query}%") ||
                    e.IncidentTaskNumber == searchQuery.Query ||
                    e.RegistrationId == searchQuery.Query ||
                    EF.Functions.Like(e.EvacuatedTo, $"%{searchQuery.Query}%") ||
                    EF.Functions.Like(e.EvacuatedFrom, $"%{searchQuery.Query}%") ||
                    (e.Dob.HasValue && e.Dob.Equals(dob)));
            }
            else
            {
                // if a search parameter is not null, then add a "where" clause to the query
                // matching the supplied UTF-16 query string
                if (!string.IsNullOrWhiteSpace(searchQuery.LastName))
                {
                    query = query.Where(e => e.LastName.Equals(searchQuery.LastName));
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.FirstName))
                {
                    query = query.Where(e => e.FirstName.Equals(searchQuery.FirstName));
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.DateOfBirth))
                {
                    // TryParse means that if it fails to parse a Date, the out value will be set to
                    // DateTime.MinVal (Midnight @ 0001 AD) Otherwise it throws an exception if it
                    // fails Letting it blow up might be more correct - Should we throw an exception
                    // if a bad date string is passed in?
                    DateTime.TryParse(searchQuery.DateOfBirth, out DateTime dob);
                    query = query.Where(e => e.Dob.Equals(dob));
                }
                // Self Registration Date Range (between start and end)
                if (!string.IsNullOrWhiteSpace(searchQuery.SelfRegistrationDateStart)
                    && !string.IsNullOrWhiteSpace(searchQuery.SelfRegistrationDateEnd))
                {
                    DateTime.TryParse(searchQuery.SelfRegistrationDateStart, out DateTime start);
                    DateTime.TryParse(searchQuery.SelfRegistrationDateEnd, out DateTime end);
                    query = query.Where(e => e.SelfRegisteredDate.HasValue &&
                                        e.SelfRegisteredDate > start && e.SelfRegisteredDate < end);
                }
                // Only start (all self registrations after start)
                else if (!string.IsNullOrWhiteSpace(searchQuery.SelfRegistrationDateStart))
                {
                    DateTime.TryParse(searchQuery.SelfRegistrationDateStart, out DateTime start);
                    query = query.Where(e => e.SelfRegisteredDate.HasValue && e.SelfRegisteredDate > start);
                }
                // Only end (all self registrations before end)
                else if (!string.IsNullOrWhiteSpace(searchQuery.SelfRegistrationDateEnd))
                {
                    DateTime.TryParse(searchQuery.SelfRegistrationDateEnd, out DateTime end);
                    query = query.Where(e => e.SelfRegisteredDate.HasValue && e.SelfRegisteredDate < end);
                }

                // Finalization date range (between start and end)
                if (!string.IsNullOrWhiteSpace(searchQuery.FinalizationDateStart)
                    && !string.IsNullOrWhiteSpace(searchQuery.FinalizationDateEnd))
                {
                    DateTime.TryParse(searchQuery.FinalizationDateStart, out DateTime start);
                    DateTime.TryParse(searchQuery.FinalizationDateEnd, out DateTime end);

                    query = query.Where(e => e.RegistrationCompletionDate.HasValue &&
                                        e.RegistrationCompletionDate.Value > start && e.RegistrationCompletionDate.Value < end);
                }
                // Only start (all finalized evacuees after start)
                else if (!string.IsNullOrWhiteSpace(searchQuery.FinalizationDateStart))
                {
                    DateTime.TryParse(searchQuery.FinalizationDateStart, out DateTime start);
                    query = query.Where(e => e.RegistrationCompletionDate.HasValue && e.RegistrationCompletionDate.Value > start);
                }
                // Only end (all finalized evacuees before end)
                else if (!string.IsNullOrWhiteSpace(searchQuery.FinalizationDateEnd))
                {
                    DateTime.TryParse(searchQuery.FinalizationDateEnd, out DateTime end);
                    query = query.Where(e => e.RegistrationCompletionDate.HasValue && e.RegistrationCompletionDate < end);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber))
                {
                    query = query.Where(e => e.IncidentTaskNumber == searchQuery.IncidentTaskNumber);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber))
                {
                    query = query.Where(e => e.RegistrationId == searchQuery.EssFileNumber);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom))
                {
                    query = query.Where(e => e.EvacuatedFrom == searchQuery.EvacuatedFrom);
                }

                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo))
                {
                    query = query.Where(e => e.EvacuatedTo == searchQuery.EvacuatedTo);
                }

                // if has referrals has a value do some things. Else is omit the where clause so it
                // is omitted
                if (searchQuery.HasReferrals.HasValue)
                {
                    // (Why can searchQuery be valueless in the object? It should take memory space
                    // whether we intantiate it or not.)
                    if (searchQuery.HasReferrals.Value)
                    {
                        // set the "where" clause for only evacuees with referrals
                        query = query.Where(e => e.HasReferrals);
                    }
                    else
                    {
                        // set the "where" clause for only evacuees without referrals
                        query = query.Where(e => !e.HasReferrals);
                    }
                }
                // allow for filtering on registration completion state
                if (searchQuery.RegistrationCompleted.HasValue)
                {
                    query = query.Where(e => e.IsFinalized == searchQuery.RegistrationCompleted.Value);
                }
            }
            return query;
        }

        public async Task<IEnumerable<EvacueeReportItem>> GetEvacueeReportAsync(EvacueeSearchQueryParameters searchQuery)
        {
            var pstTimeZone = GetPSTTimeZone();
            var query = from evacuee in db.Evacuees
                        from registration in db.EvacueeRegistrations
                            .Include(r => r.IncidentTask).ThenInclude(t => t.Community)
                            .Include(r => r.HostCommunity)
                        from primaryAddress in db.EvacueeRegistrationAddresses.Include(a => a.Community).Include(a => a.Country)
                                .Where(a => a.AddressTypeCode == "Primary" && a.RegistrationId == registration.EssFileNumber).DefaultIfEmpty()
                        from mailingAddress in db.EvacueeRegistrationAddresses.Include(a => a.Community).Include(a => a.Country)
                            .Where(a => a.AddressTypeCode == "Mailing" && a.RegistrationId == registration.EssFileNumber).DefaultIfEmpty()
                        where registration.EssFileNumber == evacuee.RegistrationId

                        select new EvacueeReportItem
                        {
                            // File Information
                            Ess_File_Number = registration.EssFileNumber,
                            Task_Number = registration.IncidentTask.TaskNumber == null ? null : registration.IncidentTask.TaskNumber,
                            //Task_Number_Start_Date = registration.IncidentTask == null ? null : registration.IncidentTask.TaskNumberStartDate.Value.LocalDateTime, //DateTime
                            Task_Number_Start_Date = registration.IncidentTask.TaskNumberStartDate.HasValue == true ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(registration.IncidentTask.TaskNumberStartDate.GetValueOrDefault().UtcDateTime, pstTimeZone) : (DateTime?)null, //DateTime
                            Task_Number_End_Date = registration.IncidentTask.TaskNumberEndDate.HasValue == true ? TimeZoneInfo.ConvertTimeBySystemTimeZoneId(registration.IncidentTask.TaskNumberEndDate.GetValueOrDefault().UtcDateTime, pstTimeZone) : (DateTime?)null, //DateTime
                            Self_Registration_Date = registration.SelfRegisteredDate.HasValue == true ? TimeZoneInfo.ConvertTimeFromUtc(registration.SelfRegisteredDate.Value, TimeZoneInfo.FindSystemTimeZoneById(pstTimeZone)) : (DateTime?)null, //DateTime
                            Registration_Completed_Date = registration.RegistrationCompletionDate.HasValue == true ? TimeZoneInfo.ConvertTimeFromUtc(registration.RegistrationCompletionDate.Value, TimeZoneInfo.FindSystemTimeZoneById(pstTimeZone)) : (DateTime?)null, //DateTime
                            File_Status = registration.IncidentTask == null ? null : registration.IncidentTask.TaskNumber == null ? "Not Finalized" : "Finalized",
                            Evacuated_From = registration.HostCommunity.Name,
                            Evacuated_To = registration.IncidentTask == null ? null : registration.IncidentTask.Community.Name,
                            Facility_Name = registration.Facility,
                            // ****** PI data start ******
                            // Evacuee Information
                            Last_Name = evacuee.LastName,
                            First_Name = evacuee.FirstName,
                            Date_Of_Birth = evacuee.Dob.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), //Date
                            Gender = evacuee.Gender,
                            Is_Head_Of_HouseHold = evacuee.EvacueeType == Enumerations.EvacueeType.HeadOfHousehold ? "Y" : "N",
                            Preferred_Name = evacuee.Nickname, //**** NEW ****
                            Initials = evacuee.Initials, //**** NEW ****
                            // Evacuee Contact Information
                            Address = primaryAddress == null ? null : primaryAddress.AddressLine1,
                            Community = primaryAddress == null ? null : primaryAddress.Community.Name,
                            Province = primaryAddress == null ? null : primaryAddress.Province,
                            Postal_Code = primaryAddress == null ? null : primaryAddress.PostalCode,
                            Country = primaryAddress == null ? null : primaryAddress.Country.Name,
                            Registration_Phone_Number = registration.PhoneNumber,
                            Registration_Alternate_Phone_Number = registration.PhoneNumberAlt,
                            Registration_Email_Address = registration.Email,
                            Mailing_Address = mailingAddress.AddressLine1 == null ? primaryAddress.AddressLine1 : mailingAddress.AddressLine1,
                            Mailing_Community = mailingAddress.Community.Name == null ? primaryAddress.Community.Name : mailingAddress.Community.Name,
                            Mailing_Province = mailingAddress.Province == null ? primaryAddress.Province : mailingAddress.Province,
                            Mailing_Postal_Code = mailingAddress.PostalCode == null ? primaryAddress.PostalCode : mailingAddress.PostalCode,
                            Mailing_Country = mailingAddress.Country.Name == null ? primaryAddress.Country.Name : mailingAddress.Country.Name,
                            // ****** PI data end ******
                            // Questions and Services
                            Insurance = registration.InsuranceCode,
                            Pets = registration.HasPets == true ? "Y" : "N",
                            Service_Recommendation_Inquiry = registration.HasInquiryReferral == true ? "Y" : "N",
                            Service_Recommendation_Health_Services = registration.HasHealthServicesReferral == true ? "Y" : "N",
                            Service_Recommendation_First_Aid = registration.HasFirstAidReferral == true ? "Y" : "N",
                            Service_Recommendation_Personal_Services = registration.HasPersonalServicesReferral == true ? "Y" : "N",
                            Service_Recommendation_Child_Care = registration.HasChildCareReferral == true ? "Y" : "N",
                            Service_Recommendation_Pet_Care = registration.HasPetCareReferral == true ? "Y" : "N",
                            // **** NEW ****
                            Requires_Accommodation = registration.RequiresAccommodation == true ? "Y" : "N",
                            Requires_Clothing = registration.RequiresClothing == true ? "Y" : "N",
                            Requires_Food = registration.RequiresFood == true ? "Y" : "N",
                            Requires_Incidentals = registration.RequiresIncidentals == true ? "Y" : "N",
                            Requires_Transportation = registration.RequiresTransportation == true ? "Y" : "N",
                            Medication_Needs = registration.MedicationNeeds == true ? "Y" : "N",
                            Medication_supply_for_3_days = registration.HasThreeDayMedicationSupply == true ? "Y" : "N",
                            Dietary_Needs = registration.DietaryNeeds == true ? "Y" : "N",
                            /* Jira EMBCESSMOD-2323
                            Pet_Care_Plan = registration.PetCarePlan,
                            Dietary_Needs_Details = registration.DietaryNeedsDetails,
                            Disaster_Affect_Details = registration.DisasterAffectDetails,
                            External_Service_Recommendations = registration.ExternalReferralsDetails,
                            Family_Recovery_Plan = registration.FamilyRecoveryPlan,
                            Internal_Case_Notes = registration.FollowUpDetails
                            */
                        };


            if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber))
            {
                query = query.Where(e => e.Task_Number == searchQuery.IncidentTaskNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber))
            {
                query = query.Where(e => e.Ess_File_Number.ToString() == searchQuery.EssFileNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo))
            {
                query = query.Where(e => e.Evacuated_From == searchQuery.EvacuatedTo);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom))
            {
                query = query.Where(e => e.Evacuated_To == searchQuery.EvacuatedFrom);
            }


            return await query.ToArrayAsync();
        }

        private string GetPSTTimeZone()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return "Pacific Standard Time";

                case PlatformID.Unix:
                    return "Canada/Pacific";

                default:
                    throw new NotSupportedException();
            }
        }

        public async Task<IEnumerable<EvacueeReportItem>> GetEvacueeReportAsync1(EvacueeSearchQueryParameters searchQuery)
        {
            var queryEvacRep = db.EvacueeReportItems
                .FromSql(@"
                    SELECT
                    -- File Information
                    evac.RegistrationId as 'ESS_File_Number',
                    task.TaskNumber as 'Task_Number',
                    CONVERT(datetime, SWITCHOFFSET(task.TaskNumberStartDate, DATEPART(TZOFFSET, task.TaskNumberStartDate AT TIME ZONE 'Pacific Standard Time'))) as 'Task_Number_Start_Date',
                    CONVERT(datetime, SWITCHOFFSET(task.TaskNumberEndDate, DATEPART(TZOFFSET, task.TaskNumberEndDate AT TIME ZONE 'Pacific Standard Time'))) as 'Task_Number_End_Date',
                    'File_Status' = CASE WHEN task.TaskNumber IS NULL THEN 'Not Finalized' ELSE 'Finalized' END,
                    commFrom.Name as 'Evacuated_From',
                    commTo.Name as 'Evacuated_To',
                    er.Facility as 'Facility_Name',
                    CONVERT(datetime, SWITCHOFFSET(er.SelfRegisteredDate, DATEPART(TZOFFSET, er.SelfRegisteredDate AT TIME ZONE 'Pacific Standard Time'))) as 'Self_Registration_Date',
                    CONVERT(datetime, SWITCHOFFSET(er.RegistrationCompletionDate, DATEPART(TZOFFSET, er.RegistrationCompletionDate AT TIME ZONE 'Pacific Standard Time'))) as 'Registration_Completed_Date',
                    -- **************** PI data start ****************
                    -- Evacuee Information
                    evac.LastName as 'Last_Name',
                    evac.FirstName as 'First_Name',
                    CAST(evac.Dob AS VARCHAR(10)) as 'Date_Of_Birth',
                    evac.Gender as 'Gender',
                    'Is_Head_Of_Household' = CASE WHEN evac.EvacueeTypeCode = 'HOH' THEN 'Y' ELSE 'N' END,
                    -- Evacuee Contact Information
                    erap.AddressLine1 as 'Address',
                    commAddr.Name as 'Community',
                    erap.Province as 'Province',
                    erap.PostalCode as 'Postal_Code',
                    countryAddr.Name as 'Country',
                    er.PhoneNumber as 'Phone_Number',
                    er.PhoneNumberAlt as 'Alternate_Phone_Number',
                    er.Email as 'Email_Address',
                    ISNULL(eram.AddressLine1, erap.AddressLine1) as 'Mailing_Address',
                    ISNULL(commAddrM.Name, commAddr.Name) as 'Mailing_Community',
                    ISNULL(eram.Province, erap.Province) as 'Mailing_Province',
                    ISNULL(eram.PostalCode, erap.PostalCode) as 'Mailing_Postal_Code',
                    ISNULL(countryAddrM.Name, countryAddr.Name) as 'Mailing_Country',
                    -- **************** PI data end ****************
                    -- Questions and Services
                    er.InsuranceCode as 'Insurance',
                    CASE WHEN er.HasPets = 1 THEN 'Y' ELSE 'N' END as 'Pets',
                    CASE WHEN er.HasInquiryReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_Inquiry',
                    CASE WHEN er.HasHealthServicesReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_Health_Services',
                    CASE WHEN er.HasFirstAidReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_First_Aid',
                    CASE WHEN er.HasPersonalServicesReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_Personal_Services',
                    CASE WHEN er.HasChildCareReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_Child_Care',
                    CASE WHEN er.HasPetCareReferral = 1 THEN 'Y' ELSE 'N' END as 'Service_Recommendation_Pet_Care'
                FROM
                    Evacuees evac
                INNER JOIN
                    EvacueeRegistrations er ON evac.RegistrationId = er.EssFileNumber
                LEFT OUTER JOIN
                    IncidentTasks task ON er.IncidentTaskId = task.Id
                LEFT OUTER JOIN
                    Communities commFrom ON er.HostCommunityId = commFrom.Id
                LEFT OUTER JOIN
                    Communities commTo ON task.CommunityId = commTo.Id
                LEFT OUTER JOIN
                    EvacueeRegistrationAddresses erap ON evac.RegistrationId = erap.RegistrationId AND erap.AddressTypeCode = 'Primary'
                LEFT OUTER JOIN
                    Communities commAddr ON commAddr.Id = erap.CommunityId
                LEFT OUTER JOIN
                    Countries countryAddr ON countryAddr.CountryCode = erap.CountryCode
                LEFT OUTER JOIN
                    EvacueeRegistrationAddresses eram ON evac.RegistrationId = eram.RegistrationId AND eram.AddressTypeCode = 'Mailing'
                LEFT OUTER JOIN
                    Communities commAddrM ON commAddrM.Id = eram.CommunityId
                LEFT OUTER JOIN
                    Countries countryAddrM ON countryAddrM.CountryCode = eram.CountryCode
                ");

            //************************************************************************
            // * START - PI columns removed as indicated in Jira ticket EMBCESSMOD-829
            //************************************************************************
            //-- Evacuee Information
            //evac.LastName as 'Last_Name',
            //evac.FirstName as 'First_Name',
            //CAST(evac.Dob AS VARCHAR(10)) as 'Date_Of_Birth',
            //evac.Gender as 'Gender',
            //'Is_Head_Of_Household' = CASE WHEN evac.EvacueeTypeCode = 'HOH' THEN 'Y' ELSE 'N' END,
            //-- Evacuee Contact Information
            //erap.AddressLine1 as 'Address',
            //commAddr.Name as 'Community',
            //erap.Province as 'Province',
            //erap.PostalCode as 'Postal_Code',
            //countryAddr.Name as 'Country',
            //er.PhoneNumber as 'Phone_Number',
            //er.PhoneNumberAlt as 'Alternate_Phone_Number',
            //er.Email as 'Email_Address',
            //ISNULL(eram.AddressLine1, erap.AddressLine1) as 'Mailing_Address',
            //ISNULL(commAddrM.Name, commAddr.Name) as 'Mailing_Community',
            //ISNULL(eram.Province, erap.Province) as 'Mailing_Province',
            //ISNULL(eram.PostalCode, erap.PostalCode) as 'Mailing_Postal_Code',
            //ISNULL(countryAddrM.Name, countryAddr.Name) as 'Mailing_Country',
            //**********************************************************************
            // * END - PI columns removed as indicated in Jira ticket EMBCESSMOD-829
            //**********************************************************************

            //Apply Where clauses
            if (!string.IsNullOrWhiteSpace(searchQuery.LastName))
            {
                queryEvacRep = queryEvacRep.Where(e => e.Last_Name.Equals(searchQuery.LastName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.FirstName))
            {
                queryEvacRep = queryEvacRep.Where(e => e.First_Name.Equals(searchQuery.FirstName, StringComparison.OrdinalIgnoreCase));
            }

            //if (!string.IsNullOrWhiteSpace(searchQuery.DateOfBirth))
            //{
            //    // TryParse means that if it fails to parse a Date, the out value will be set to
            //    // DateTime.MinVal (Midnight @ 0001 AD) Otherwise it throws an exception if it
            //    // fails Letting it blow up might be more correct - Should we throw an exception
            //    // if a bad date string is passed in?
            //    DateTime.TryParse(searchQuery.DateOfBirth, out DateTime dob);
            //    query = query.Where(e => DateTime.Parse(e.Date_Of_Birth).Equals(dob));
            //}

            if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber))
            {
                queryEvacRep = queryEvacRep.Where(e => e.Task_Number == searchQuery.IncidentTaskNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber))
            {
                queryEvacRep = queryEvacRep.Where(e => e.Ess_File_Number.ToString() == searchQuery.EssFileNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo))
            {
                queryEvacRep = queryEvacRep.Where(e => e.Evacuated_From == searchQuery.EvacuatedTo);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom))
            {
                queryEvacRep = queryEvacRep.Where(e => e.Evacuated_To == searchQuery.EvacuatedFrom);
            }

            return await queryEvacRep.ToListAsync();
        }

        public async Task<IEnumerable<Models.Db.ReferralReportItem>> GetEvacueeReferralReportAsync(EvacueeSearchQueryParameters searchQuery)
        {
            var query = db.ReferralReportItems
                .FromSql(@"
                    select
                        -- File Information
                        ref.RegistrationId as 'ESS_File_Number',
                        task.TaskNumber as 'Task_Number',
                        CONVERT(datetime, SWITCHOFFSET(task.TaskNumberStartDate, DATEPART(TZOFFSET, task.TaskNumberStartDate AT TIME ZONE 'Pacific Standard Time'))) as 'Task_Number_Start_Date',
                        CONVERT(datetime, SWITCHOFFSET(task.TaskNumberEndDate, DATEPART(TZOFFSET, task.TaskNumberEndDate AT TIME ZONE 'Pacific Standard Time'))) as 'Task_Number_End_Date',
                        'File_Status' = CASE WHEN task.TaskNumber IS NULL THEN 'Not Finalized' ELSE 'Finalized' END,
                        commFrom.Name as 'Evacuated_From',
                        commTo.Name as 'Evacuated_To',
                        evareg.Facility as 'Facility_Name',
                        CONVERT(datetime, SWITCHOFFSET(evareg.SelfRegisteredDate, DATEPART(TZOFFSET, evareg.SelfRegisteredDate AT TIME ZONE 'Pacific Standard Time'))) as 'Self_Registration_Date',
                        CONVERT(datetime, SWITCHOFFSET(evareg.RegistrationCompletionDate, DATEPART(TZOFFSET, evareg.RegistrationCompletionDate AT TIME ZONE 'Pacific Standard Time'))) as 'Registration_Completed_Date',
                        -- Referral Referenced User
                        ref.Purchaser AS 'Person_responsible_for_purchasing_goods',
                        -- Referral
                        ref.Id as 'Referral_Number',
                        -- ******************* PI columns start
                        LEFT(ref.Type, CASE WHEN charindex('_', ref.Type) = 0 THEN LEN(ref.Type) ELSE charindex('_', ref.Type) - 1 END) as 'Support_Type',
                        CASE WHEN charindex('_', ref.Type) = 0 THEN '' ELSE Substring(ref.Type, Charindex('_', ref.Type)+1, Len(ref.Type)) END as 'Sub_Support_Type',
                        CONVERT(datetime, SWITCHOFFSET(ref.ValidFrom, DATEPART(TZOFFSET, ref.ValidFrom AT TIME ZONE 'Pacific Standard Time'))) as 'Valid_From_Date',
                        DATEDIFF(DAY, ref.ValidFrom, ref.ValidTo) as 'Number_Of_Days',
                        CONVERT(datetime, SWITCHOFFSET(ref.ValidTo, DATEPART(TZOFFSET, ref.ValidTo AT TIME ZONE 'Pacific Standard Time'))) as 'Valid_To_Date',
                        (select count(1) from ReferralEvacuees where ReferralId = ref.id) as 'Number_Of_Evacuees_for_Referral',
                        ref.TotalAmount as 'Total_Amount',
                        ISNULL(ref.NumberOfBreakfasts, 0) as 'Breakfasts_per_person',
                        ISNULL(ref.NumberOfLunches, 0) as 'Lunches_per_person',
                        ISNULL(ref.NumberOfDinners, 0) as 'Dinners_per_person',
                        ISNULL(ref.NumberOfRooms, 0) as 'Number_of_Rooms',
                        CASE WHEN ref.Type = 'Lodging_Hotel' THEN ISNULL(ref.HotelLodgingReferral_NumberOfNights, 0)
                            ELSE CASE WHEN ref.Type = 'Lodging_Group' THEN ISNULL(ref.GroupLodgingReferral_NumberOfNights, 0) END END as 'Number_of_Nights',
                        CONVERT(datetime, SWITCHOFFSET(ref.CreatedDateTime, DATEPART(TZOFFSET, ref.CreatedDateTime AT TIME ZONE 'Pacific Standard Time'))) as 'Referral_Created_Date', --****NEW**** EMBCESSMOD-2076
                        'Clothing_Extreme_Winter_Conditions' = CASE ref.ExtremeWinterConditions WHEN 1 THEN 'Y' WHEN 0 THEN 'N' ELSE null END, --****NEW****
                        -------- Jira EMBCESSMOD-2323 potential fields with characters affecting the csv file output
                        --'Mode_of_Transportation' = ref.TransportMode,
                        --'Referral_Comments' = ref.Comments, --****NEW****
                        --'Taxi_From_Address' = CASE ref.Type WHEN 'Transportation_Taxi' THEN ref.FromAddress END, --****NEW****
                        --'Taxi_To_Address' = CASE ref.Type WHEN 'Transportation_Taxi' THEN ref.ToAddress END, --****NEW****
                        --'Incidentals_Approved_Items' = CASE ref.Type WHEN 'Incidentals' THEN ref.ApprovedItems END, --****NEW****
                        -------- Jira EMBCESSMOD-2323
                        'Groceries_Number_Of_Meals' = CASE ref.Type WHEN 'Food_Groceries' THEN ref.NumberOfMeals END, --****NEW****
                        --Referrals Supplier
                        sup.Name as 'Supplier_Name',
                        sup.Address as 'Supplier_Address',
                        sup.City as 'Supplier_City',
                        sup.PostalCode as 'Supplier_Postal_Code',
                        sup.Telephone as 'Supplier_Telephone',
                        sup.Fax as 'Supplier_Fax'
                        -- ******************* PI columns end
                    from Referrals ref
                        INNER JOIN Suppliers sup on ref.SupplierId = sup.Id
                        INNER JOIN EvacueeRegistrations evareg on ref.RegistrationId = evareg.EssFileNumber
                        INNER JOIN IncidentTasks task on evareg.IncidentTaskId = task.Id
                        LEFT OUTER JOIN Communities commFrom ON evareg.HostCommunityId = commFrom.Id
                        LEFT OUTER JOIN Communities commTo ON task.CommunityId = commTo.Id
                    where ref.Active = 1
                ");
            /*****************************************************************
             * PI columns removed as indicated in Jira ticket EMBCESSMOD-745
             *****************************************************************
            LEFT(ref.Type, CASE WHEN charindex('_', ref.Type) = 0 THEN LEN(ref.Type) ELSE charindex('_', ref.Type) - 1 END) as 'Support_Type',
            CASE WHEN charindex('_', ref.Type) = 0 THEN '' ELSE Substring(ref.Type, Charindex('_', ref.Type)+1, Len(ref.Type)) END as 'Sub_Support_Type',
            CONVERT(datetime, SWITCHOFFSET(ref.ValidFrom, DATEPART(TZOFFSET, ref.ValidFrom AT TIME ZONE 'Pacific Standard Time'))) as 'Valid_From_Date',
            DATEDIFF(DAY, ref.ValidFrom, ref.ValidTo) as 'Number_Of_Days',
            CONVERT(datetime, SWITCHOFFSET(ref.ValidTo, DATEPART(TZOFFSET, ref.ValidTo AT TIME ZONE 'Pacific Standard Time'))) as 'Valid_To_Date',
            (select count(1) from ReferralEvacuees where ReferralId = ref.id) as 'Number_Of_Evacuees_for_Referral',
            ref.TotalAmount as 'Total_Amount',
            ISNULL(ref.NumberOfBreakfasts, 0) as 'Breakfasts_per_person',
            ISNULL(ref.NumberOfLunches, 0) as 'Lunches_per_person',
            ISNULL(ref.NumberOfDinners, 0) as 'Dinners_per_person',
            ISNULL(ref.NumberOfRooms, 0) as 'Number_of_Rooms',
            CASE WHEN ref.Type = 'Lodging_Hotel' THEN ISNULL(ref.HotelLodgingReferral_NumberOfNights, 0)
                ELSE CASE WHEN ref.Type = 'Lodging_Group' THEN ISNULL(ref.GroupLodgingReferral_NumberOfNights, 0) END END as 'Number_of_Nights',
            ref.TransportMode as 'Mode_of_Transportation',
            --Referrals Supplier
            sup.Name as 'Supplier_Name',
            sup.Address as 'Supplier_Address',
            sup.City as 'City',
            sup.PostalCode as 'Postal_Code',
            sup.Telephone as 'Telephone',
            sup.Fax as 'Fax'
            */

                            // Apply where clauses
            if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber))
            {
                query = query.Where(e => e.Task_Number == searchQuery.IncidentTaskNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber))
            {
                query = query.Where(e => e.Ess_File_Number.ToString() == searchQuery.EssFileNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo))
            {
                query = query.Where(e => e.Evacuated_From == searchQuery.EvacuatedTo);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom))
            {
                query = query.Where(e => e.Evacuated_To == searchQuery.EvacuatedFrom);
            }

            return await query.ToListAsync();
        }

        private string MapSortToFields(string sort)
        {
            return sort
                    .Replace("evacuatedFrom", "EvacuatedFrom", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("evacuatedTo", "EvacuatedTo", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("essFileNumber", "RegistrationId", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
