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
            var query = db.ViewEvacuees
                .Where(e => e.Active == searchQuery.Active);

            if (searchQuery.HasQuery())
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.LastName, $"%{searchQuery.Query}%") ||
                    e.IncidentTaskNumber == searchQuery.Query ||
                    e.RegistrationId == searchQuery.Query ||
                    EF.Functions.Like(e.EvacuatedTo, $"%{searchQuery.Query}%") ||
                    EF.Functions.Like(e.EvacuatedFrom, $"%{searchQuery.Query}%"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchQuery.LastName)) query = query.Where(e => EF.Functions.Like(e.LastName, $"%{searchQuery.LastName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.FirstName)) query = query.Where(e => EF.Functions.Like(e.FirstName, $"%{searchQuery.FirstName}%"));
                if (!string.IsNullOrWhiteSpace(searchQuery.IncidentTaskNumber)) query = query.Where(e => e.IncidentTaskNumber == searchQuery.IncidentTaskNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EssFileNumber)) query = query.Where(e => e.RegistrationId == searchQuery.EssFileNumber);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedFrom)) query = query.Where(e => e.EvacuatedFrom == searchQuery.EvacuatedFrom);
                if (!string.IsNullOrWhiteSpace(searchQuery.EvacuatedTo)) query = query.Where(e => e.EvacuatedTo == searchQuery.EvacuatedTo);
                if (searchQuery.HasReferrals.HasValue && searchQuery.HasReferrals.Value) query = query.Where(e => e.HasReferrals);
                if (searchQuery.HasReferrals.HasValue && !searchQuery.HasReferrals.Value) query = query.Where(e => !e.HasReferrals);
                if (searchQuery.RegistrationCompleted.HasValue) query = query.Where(e => e.IsFinalized == searchQuery.RegistrationCompleted.Value);
            }

            if (!searchQuery.HasSortBy()) searchQuery.SortBy = "-essFileNumber";

            var pagedQuery = new PaginatedQuery<Models.Db.ViewEvacuee>(query, searchQuery.Offset, searchQuery.Limit);
            query = pagedQuery.Query;

            var results = await query.Sort(MapSortToFields(searchQuery.SortBy)).ToArrayAsync();

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
