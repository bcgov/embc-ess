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
        private IQueryable<Models.Db.IncidentTask> IncidentTasks => db.IncidentTasks
                .Include(t => t.Region)
                .Include(t => t.Community)
                    .ThenInclude(d => d.Region);

        public async Task<IPagedResults<IncidentTask>> GetIncidentTasksAsync(SearchQueryParameters searchQuery)
        {
            var items = await IncidentTasks
                .GroupJoin(db.EvacueeRegistrations.Select(e => new { e.IncidentTaskId, evacueeCount = e.Evacuees.Count() }),
                    incident => incident.Id,
                    summary => summary.IncidentTaskId,
                    (incident, summary) => new { incident = incident, evacueeCount = summary.Sum(s => s.evacueeCount) }
                )
                .Where(t => !searchQuery.HasQuery() || t.incident.Community.Id == Guid.Parse(searchQuery.Query))
                .Where(t => searchQuery.Active == t.incident.Active)
                .Sort(searchQuery.SortBy ?? "incident.id")
                .ToArrayAsync();

            return new PaginatedList<IncidentTask>(items.Select(t => t.incident.ToViewModel(t.evacueeCount)), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<IncidentTask> GetIncidentTaskAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                var entity = await IncidentTasks.SingleOrDefaultAsync(task => task.Id == guid);
                return entity?.ToViewModel();
            }
            return null;
        }

        public async Task<IncidentTask> CreateIncidentTaskAsync(IncidentTask task)
        {
            var newItem = db.IncidentTasks.Add(task.ToModel());
            await db.SaveChangesAsync();
            return (await IncidentTasks.SingleAsync(t => t.Id == newItem.Entity.Id)).ToViewModel();
        }

        public async Task UpdateIncidentTaskAsync(IncidentTask task)
        {
            db.IncidentTasks.Update(task.ToModel());
            await db.SaveChangesAsync();
        }

        public async Task<bool> DeactivateIncidentTaskAsync(string id)
        {
            var entity = await db.IncidentTasks.SingleAsync(x => x.Id == Guid.Parse(id));
            if (entity == null)
            {
                return false;
            }
            entity.Active = false;
            db.IncidentTasks.Update(entity);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
