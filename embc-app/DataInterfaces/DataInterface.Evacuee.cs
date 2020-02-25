using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IPagedResults<EvacueeListItem>> GetEvacueesAsync(EvacueeSearchQueryParameters searchQuery)
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
                // Inactive evacuees are soft deleted. We do not return them or give the user the option yet.
                .Where(e => e.Active == searchQuery.Active)
                // we sort the larger collection first before letting the subset (paginated ones be sorted)
                .Sort(MapSortToFields(searchQuery.SortBy));


            if (searchQuery.HasQuery())
            {
                // Try to parse the query as a date - if it fails it sets dob to DateTime.MinValue (Midnight @ 0001 AD), which shouldn't match anyone
                DateTime.TryParse(searchQuery.Query, out DateTime dob);

                // Simple search. When a search query is provided search should match multiple things from the record. Query can match multiple things.
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
                // if a search parameter is not null, then add a "where" clause to the query matching the supplied UTF-16 query string
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
                    // TryParse means that if it fails to parse a Date, the out value will be set to DateTime.MinVal (Midnight @ 0001 AD)
                    // Otherwise it throws an exception if it fails
                    // Letting it blow up might be more correct - Should we throw an exception if a bad date string is passed in?
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

                // Finalization date range  (between start and end)
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

                // if has referrals has a value do some things. Else is omit the where clause so it is omitted
                if (searchQuery.HasReferrals.HasValue)
                {
                    // (Why can searchQuery be valueless in the object? It should take memory space whether we intantiate it or not.)
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

            // build the paginated query
            var pagedQuery = new PaginatedQuery<Models.Db.ViewEvacuee>(query, searchQuery.Offset, searchQuery.Limit);

            // get results back from 
            var results = await pagedQuery.Query.Sort(MapSortToFields(searchQuery.SortBy)).ToArrayAsync();

            // map the evacueeList
            return new PaginatedList<EvacueeListItem>(results.Select(mapper.Map<EvacueeListItem>), pagedQuery.Pagination);
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
