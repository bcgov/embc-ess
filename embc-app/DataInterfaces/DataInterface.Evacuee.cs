using AutoMapper.QueryableExtensions;
using Gov.Jag.Embc.Public.Models.Db;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        //Task<IEnumerable<EvacueeReportItem>> GetEvacueeReportAsync(EvacueeSearchQueryParameters query)
        //{
        //    return await db.EvacueeReportItems
        //        .FromSql(@"")
        //        .ToList();
        //}

        public async Task<IEnumerable<EvacueeReportItem>> GetEvacueeReport(EvacueeSearchQueryParameters query)
        {
            return await db.EvacueeReportItems
                .FromSql(@"
                    SELECT
	                -- File Information
	                evac.RegistrationId as 'ESS_File_Number',
	                task.TaskNumber as 'Task_Number',
	                task.TaskNumberStartDate as 'Task_Number_Start_Date',
	                task.TaskNumberEndDate as 'Task_Number_End_Date',
	                'File_Status' = CASE WHEN task.TaskNumber IS NULL THEN 'Not Finalized' ELSE 'Finalized' END,
	                commTo.Name as 'Evacuated_To', --ISNULL(commTo.Name, task.RegionName) as 'Evacuated To',
	                commFrom.Name as 'Evacuated_From', --ISNULL(commFrom.Name, task.RegionName) as 'Evacuated From',
	                er.Facility as 'Facility_Name',
	                CONVERT(date, er.SelfRegisteredDate) as 'Self_Registration_Date',
	                CONVERT(time, er.SelfRegisteredDate) as 'Self_Registration_Time',
	                CONVERT(date, er.RegistrationCompletionDate) as 'Registration_Completed_Date',
	                CONVERT(time, er.RegistrationCompletionDate) as 'Registration_Completed_Time',
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
                    Communities commFrom ON task.CommunityId = commFrom.Id
                LEFT OUTER JOIN
                    Communities commTo ON er.HostCommunityId = commTo.Id
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
                order by evac.RegistrationId desc
                ")
                .ToListAsync();
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
