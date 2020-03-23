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

            return new PaginatedList<IncidentTask>(items.Select(i => mapper.Map<IncidentTask>(i.incident, opts => opts.Items["EvacueeCount"] = i.evacueeCount)),
                searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<IncidentTask> GetIncidentTaskAsync(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                var entity = await IncidentTasks
                    .Select(incident => new { incident, evacueeCount = incident.EvacueeRegistrations.Select(er => er.Evacuees.Count()).Sum() })
                    .SingleOrDefaultAsync(task => task.incident.Id == guid);

                return mapper.Map<IncidentTask>(entity.incident, opts => opts.Items["EvacueeCount"] = entity.evacueeCount);
            }
            return null;
        }

        public async Task<IPagedResults<IncidentTask>> GetOpenIncidentTasksAsync(int limit = 100, int offset = 0)
        {
            // Tasks are considered open if their TaskNumberEndDate is in the future
            DateTime now = DateTime.UtcNow;
            var items = await IncidentTasks
                        .Where(i => i.Active && i.TaskNumberEndDate.HasValue && i.TaskNumberEndDate > now)//.Include("Community")
                        .ToArrayAsync();

            return new PaginatedList<IncidentTask>(items.Select(i => mapper.Map<IncidentTask>(i)), offset, limit);
        }

        public async Task<PaginationMetadata> GetOpenIncidentTasksMetadataAsync(int limit = 100, int offset = 0)
        {
            DateTime now = DateTime.UtcNow;
            int count = await IncidentTasks
                        .Where(i => i.Active && i.TaskNumberEndDate.HasValue && i.TaskNumberEndDate > now)
                        .CountAsync();
            // Build the meta data
            var result = new PaginationMetadata()
            {
                CurrentPage = (int)Math.Floor((decimal)offset / limit) + 1,
                PageSize = limit,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling((decimal)count / limit)
            };
            return result;
        }

        public async Task<PaginationMetadata> GetClosedIncidentTasksMetadataAsync(int limit = 100, int offset = 0)
        {
            DateTime now = DateTime.UtcNow;
            int count = await IncidentTasks
                        .Where(i => i.Active && i.TaskNumberEndDate.HasValue && i.TaskNumberEndDate < now)
                        .CountAsync();
            // Build the meta data
            var result = new PaginationMetadata()
            {
                CurrentPage = (int)Math.Floor((decimal)offset / limit) + 1,
                PageSize = limit,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling((decimal)count / limit)
            };
            return result;
        }

        public async Task<IncidentTask> GetIncidentTaskByTaskNumbetAsync(string taskNumber)
        {
            var entity = await IncidentTasks
                .SingleOrDefaultAsync(task => task.TaskNumber == taskNumber);
            return mapper.Map<IncidentTask>(entity);
        }

        public async Task<string> CreateIncidentTaskAsync(IncidentTask task)
        {
            var newItem = db.IncidentTasks.Add(mapper.Map<Models.Db.IncidentTask>(task));
            await db.SaveChangesAsync();
            return newItem.Entity.Id.ToString();
        }

        public async Task UpdateIncidentTaskAsync(IncidentTask task)
        {
            db.IncidentTasks.Update(mapper.Map<Models.Db.IncidentTask>(task));
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
