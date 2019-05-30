using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IPagedResults<EvacueeListItem>> GetEvacueesAsync(SearchQueryParameters searchQuery)
        {
            var query = db.Evacuees
                .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.IncidentTask)
                        .ThenInclude(i => i.Community)
                .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.IncidentTask)
                        .ThenInclude(i => i.Region)
               .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.HostCommunity)
                .Include(e => e.Referrals)
                .Where(e => e.EvacueeRegistration.Active == searchQuery.Active);

            if (searchQuery.HasQuery()) query = query.Where(e =>
                EF.Functions.Like(e.LastName, $"{searchQuery.Query}%") ||
                e.EvacueeRegistration.IncidentTask.TaskNumber == searchQuery.Query ||
                e.EvacueeRegistration.EssFileNumber.ToString() == searchQuery.Query ||
                EF.Functions.Like(e.EvacueeRegistration.HostCommunity.Name, $"%{searchQuery.Query}%") ||
                EF.Functions.Like(e.EvacueeRegistration.IncidentTask.Community.Name, $"%{searchQuery.Query}%")
            );

            if (!searchQuery.HasSortBy()) searchQuery.SortBy = "-essFileNumber";

            var results = await query.Sort(MapSortToFields(searchQuery.SortBy)).ToArrayAsync();

            return new PaginatedList<EvacueeListItem>(results.Select(mapper.Map<EvacueeListItem>), searchQuery.Offset, searchQuery.Limit);
        }

        private string MapSortToFields(string sort)
        {
            return sort
                    .Replace("evacuatedFrom", "EvacueeRegistration.IncidentTask.Community.Name", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("evacuatedTo", "EvacueeRegistration.HostCommunity.Name", StringComparison.InvariantCultureIgnoreCase)
                    .Replace("essFileNumber", "EvacueeRegistration.EssFileNumber", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
