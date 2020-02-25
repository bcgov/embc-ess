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
        private IQueryable<Models.Db.VolunteerTask> VolunteerTasks => db.VolunteerTasks
            .AsNoTracking()
            .Include(v => v.Volunteer)
            .Include(v => v.IncidentTask);

        public async Task UpdateVolunteerTasksAsync(VolunteerTask updatedVolunteerTask)
        {
            var volunteerTask = mapper.Map<Models.Db.VolunteerTask>(updatedVolunteerTask);
            db.VolunteerTasks.Update(volunteerTask);
            await db.SaveChangesAsync();
        }

          public async Task<VolunteerTask> GetVolunteerTaskByIdAsync(int id)
        {
            var volunteerTask = await VolunteerTasks.SingleOrDefaultAsync(v => v.Id == id);
            return mapper.Map<VolunteerTask>(volunteerTask);
        }

          public async Task<VolunteerTask> GetVolunteerTaskByIncideTaskIdAsync(Guid taskId)
        {
            var volunteerTask = await VolunteerTasks.SingleOrDefaultAsync(v => v.IncidentTask.Id == taskId);
            return mapper.Map<VolunteerTask>(volunteerTask);
        }

        public async Task<string> CreateVolunteerTaskAsync(VolunteerTask newVolunteerTask)
        {
            // if (newVolunteerTask.IncidentTaskId == null) throw new InvalidOperationException($"VolunteerTasks {newVolunteerTask.Id} is not associated with an organization");
            var volunteerTask = mapper.Map<Models.Db.VolunteerTask>(newVolunteerTask);
            var newEntity = await db.VolunteerTasks.AddAsync(volunteerTask);
            await db.SaveChangesAsync();
            return newEntity.Entity.Id.ToString();
        }

    }
}