using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.IO;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Gov.Embc.Public.Seeders
{
    public class IncidentTaskSeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public IncidentTaskSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateIncidentTasks(context);
        }

        public override int InvokeOrder => 3;

        private void UpdateIncidentTasks(EmbcDbContext context)
        {
            List<IncidentTask> seedEntities = GetSeedIncidentTasks();

            foreach (var item in seedEntities)
            {
                CreateOrUpdateFromSeedEntity(context, item);
            }

            AddInitialIncidentTasks(context);
        }

        private void CreateOrUpdateFromSeedEntity(EmbcDbContext context, IncidentTask seedData)
        {
            IncidentTask existing = context.IncidentTasks.FirstOrDefault(x => x.TaskNumber == seedData.TaskNumber);
            if (existing == null)
            {
                context.IncidentTasks.Add(seedData);
            }
            else
            {
                existing.TaskNumber = seedData.TaskNumber;
                existing.Details = seedData.Details;
                existing.Active = seedData.Active;
                existing.Community = seedData.Community;
                existing.RegionalDistrict = seedData.RegionalDistrict;
                existing.Region = seedData.Region;
                context.IncidentTasks.Update(existing);
            }
            context.SaveChanges();
        }

        private void AddInitialIncidentTasks(EmbcDbContext context)
        {
            string filename = Configuration["IncidentTaskInitializationFilename"];
            if (string.IsNullOrEmpty(filename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "IncidentTasks.json");
            }
            context.AddInitialIncidentTasksFromFile(filename);
        }

        private List<IncidentTask> GetSeedIncidentTasks()
        {
            List<IncidentTask> entities = new List<IncidentTask>(GetDefaultIncidentTasks());

            if (IsProductionEnvironment)
            {
                entities.AddRange(GetProdIncidentTasks());
            }
            else
            {
                entities.AddRange(GetDevIncidentTasks());
            }

            return entities;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<IncidentTask> GetDefaultIncidentTasks()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<IncidentTask> GetDevIncidentTasks()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<IncidentTask> GetTestIncidentTasks()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<IncidentTask> GetProdIncidentTasks()
        {
            return new List<IncidentTask>();
        }
    }
}
