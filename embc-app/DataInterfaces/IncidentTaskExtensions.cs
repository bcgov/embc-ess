using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class IncidentTaskExtensions
    {
        public static void AddIncidentTask(this EmbcDbContext context, IncidentTask IncidentTask)
        {
            // create a new IncidentTask.
            context.IncidentTasks.Add(IncidentTask);
            context.SaveChanges();
        }

        /// <summary>
        /// Returns a specific IncidentTask
        /// </summary>
        /// <param name="taskNumber">The task number of the IncidentTask</param>
        /// <returns>The IncidentTask, or null if it does not exist.</returns>
        public static IncidentTask GetIncidentTaskByTaskNumber(this EmbcDbContext context, string taskNumber)
        {
            IncidentTask IncidentTask = context.IncidentTasks.FirstOrDefault(x => x.TaskNumber == taskNumber);
            return IncidentTask;
        }

        /// <summary>
        /// Create IncidentTasks from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="IncidentTaskJsonPath"></param>
        public static void AddInitialIncidentTasksFromFile(this EmbcDbContext context, string IncidentTaskJsonPath)
        {
            if (!string.IsNullOrEmpty(IncidentTaskJsonPath) && File.Exists(IncidentTaskJsonPath))
            {
                string IncidentTaskJson = File.ReadAllText(IncidentTaskJsonPath);
                context.AddInitialIncidentTasks(IncidentTaskJson);
            }
        }

        private static void AddInitialIncidentTasks(this EmbcDbContext context, string IncidentTaskJson)
        {
            List<IncidentTask> IncidentTasks = JsonConvert.DeserializeObject<List<IncidentTask>>(IncidentTaskJson);

            if (IncidentTasks != null)
            {
                context.AddInitialIncidentTasks(IncidentTasks);
            }
        }

        private static void AddInitialIncidentTasks(this EmbcDbContext context, List<IncidentTask> IncidentTasks)
        {
            IncidentTasks.ForEach(context.AddInitialIncidentTask);
        }

        /// <summary>
        /// Adds a IncidentTask to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialIncidentTask(this EmbcDbContext context, IncidentTask initialIncidentTask)
        {
            IncidentTask IncidentTask = context.GetIncidentTaskByTaskNumber(initialIncidentTask.TaskNumber);
            if (IncidentTask != null)
            {
                return;
            }

            Region region = null;

            if (initialIncidentTask.Region != null)
            {
                region = context.GetRegionByName(initialIncidentTask.Region.Name);
            }

            Community community = null;

            if (initialIncidentTask.Community != null)
            {
                community = context.GetCommunityByName(initialIncidentTask.Community.Name);
            }

            IncidentTask = new IncidentTask
            ()
            {
                Details = initialIncidentTask.Details,
                Active = true,
                Region = region,
                Community = community,
                TaskNumber = initialIncidentTask.TaskNumber
            };

            context.AddIncidentTask(IncidentTask);
        }
    }
}
