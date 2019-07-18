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
            if (searchQuery.HasSortBy()) {
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
                // Simple search. When a search query is provided search should match multiple things from the record. Query can match multiple things.
                query = query.Where(e =>
                    EF.Functions.Like(e.LastName, $"%{searchQuery.Query}%") ||
                    e.IncidentTaskNumber == searchQuery.Query ||
                    e.RegistrationId == searchQuery.Query ||
                    EF.Functions.Like(e.EvacuatedTo, $"%{searchQuery.Query}%") ||
                    EF.Functions.Like(e.EvacuatedFrom, $"%{searchQuery.Query}%"));
            }
            else
            {
                // Advanced search. There is a null query. However the client has supplied other specific parameters to search by.
                // if a search parameter is not null, then add a "where" clause to the query matching the supplied UTF-16 query string
                if (!string.IsNullOrWhiteSpace(searchQuery.LastName)) query = query.Where(e => EF.Functions.Like(e.LastName, $"%{searchQuery.LastName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.FirstName)) query = query.Where(e => EF.Functions.Like(e.FirstName, $"%{searchQuery.FirstName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber)) query = query.Where(e => e.IncidentTaskNumber == searchQuery.IncidentTaskNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber)) query = query.Where(e => e.RegistrationId == searchQuery.EssFileNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom)) query = query.Where(e => e.EvacuatedFrom == searchQuery.EvacuatedFrom);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo)) query = query.Where(e => e.EvacuatedTo == searchQuery.EvacuatedTo);

                // if has referrals has a value do some things. Else is omit the where clause so it is omitted
                if (searchQuery.HasReferrals.HasValue)
                {
                    // (Why can searchQuery be valueless in the object? It should take memory space whether we intantiate it or not.)
                    if (searchQuery.HasReferrals.Value) {
                        // set the "where" clause for only evacuees with referrals
                        query = query.Where(e => e.HasReferrals);
                    }
                    else
                    {
                        // set the "where" clause for only evacuees without referrals
                        query = query.Where(e => !e.HasReferrals);
                    }
                }
                // If the 
                if (searchQuery.RegistrationCompleted.HasValue) query = query.Where(e => e.IsFinalized == searchQuery.RegistrationCompleted.Value);
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
