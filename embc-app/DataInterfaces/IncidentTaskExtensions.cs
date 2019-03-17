using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class IncidentTaskExtensions
    {
        public static void AddIncidentTask(this SqliteContext context, IncidentTask IncidentTask)
        {
            // create a new IncidentTask.           
            context.IncidentTasks.Add(IncidentTask);
            context.SaveChanges();
        }

        public static void UpdateIncidentTask(this SqliteContext context, IncidentTask IncidentTask)
        {
            IncidentTask _IncidentTask = context.IncidentTasks.FirstOrDefault<IncidentTask>(x => x.Id == IncidentTask.Id);
            _IncidentTask.Details = IncidentTask.Details;
            context.IncidentTasks.Update(_IncidentTask);
            context.SaveChanges();
        }

        public static List<IncidentTask> GetIncidentTasks(this SqliteContext context)
        {
            List<IncidentTask> IncidentTasks =
                context.IncidentTasks.ToList<IncidentTask>();
            return IncidentTasks;
        }

        /// <summary>
        /// Returns a specific IncidentTask
        /// </summary>
        /// <param name="details">The details of the IncidentTask</param>
        /// <returns>The IncidentTask, or null if it does not exist.</returns>
        public static IncidentTask GetIncidentTaskByDetails(this SqliteContext context, string details)
        {
            IncidentTask IncidentTask = context.IncidentTasks.FirstOrDefault(x => x.Details == details);
            return IncidentTask;
        }



        /// <summary>
        /// Create IncidentTasks from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="IncidentTaskJsonPath"></param>
        public static void AddInitialIncidentTasksFromFile(this SqliteContext context, string IncidentTaskJsonPath)
        {
            if (!string.IsNullOrEmpty(IncidentTaskJsonPath) && File.Exists(IncidentTaskJsonPath))
            {
                string IncidentTaskJson = File.ReadAllText(IncidentTaskJsonPath);
                context.AddInitialIncidentTasks(IncidentTaskJson);
            }
        }

        private static void AddInitialIncidentTasks(this SqliteContext context, string IncidentTaskJson)
        {
            List<IncidentTask> IncidentTasks = JsonConvert.DeserializeObject<List<IncidentTask>>(IncidentTaskJson);

            if (IncidentTasks != null)
            {
                context.AddInitialIncidentTasks(IncidentTasks);
            }
        }

        private static void AddInitialIncidentTasks(this SqliteContext context, List<IncidentTask> IncidentTasks)
        {
            IncidentTasks.ForEach(context.AddInitialIncidentTask);
        }

        /// <summary>
        /// Adds a IncidentTask to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialIncidentTask(this SqliteContext context, IncidentTask initialIncidentTask)
        {
            IncidentTask IncidentTask = context.GetIncidentTaskByDetails(initialIncidentTask.Details);
            if (IncidentTask != null)
            {
                return;
            }

            Region region = null;

            if (initialIncidentTask.Region != null)
            {
                region = context.GetRegionByName(initialIncidentTask.Region.Name);
            }

            RegionalDistrict regionalDistrict = null;

            if (initialIncidentTask.RegionalDistrict != null)
            {
                regionalDistrict = context.GetRegionalDistrictByName(initialIncidentTask.RegionalDistrict.Name);
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
                RegionalDistrict = regionalDistrict,
                Community = community,
                TaskNumber = initialIncidentTask.TaskNumber
            };

            context.AddIncidentTask(IncidentTask);
        }


        /// <summary>
        /// Update IncidentTask
        /// </summary>
        /// <param name="context"></param>
        /// <param name="IncidentTaskInfo"></param>
        public static void UpdateSeedIncidentTaskInfo(this SqliteContext context, IncidentTask IncidentTaskInfo)
        {
            IncidentTask IncidentTask = context.GetIncidentTaskByDetails(IncidentTaskInfo.Details);
            if (IncidentTask == null)
            {
                context.AddIncidentTask(IncidentTaskInfo);
            }
            else
            {
                IncidentTask.Details = IncidentTaskInfo.Details;
                IncidentTask.Active = IncidentTaskInfo.Active;
                IncidentTask.TaskNumber = IncidentTaskInfo.TaskNumber;
                context.UpdateIncidentTask(IncidentTask);
            }
        }
    }
}
