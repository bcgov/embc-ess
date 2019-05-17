using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        public async Task<IPagedResults<EvacueeListItem>> GetEvacueesAsync(SearchQueryParameters query)
        {
            var results = await db.Evacuees
                .Include(e => e.EvacueeRegistration)
                    .ThenInclude(r => r.IncidentTask)
                .Where(e => e.EvacueeRegistration.Active == query.Active)
                .ToArrayAsync();

            return new PaginatedList<EvacueeListItem>(results.Select(mapper.Map<EvacueeListItem>), query.Offset, query.Limit);
        }
    }
}
