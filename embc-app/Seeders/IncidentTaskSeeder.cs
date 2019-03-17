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
    public class IncidentTaskSeeder : Seeder<SqliteContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public IncidentTaskSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(SqliteContext context)
        {
            UpdateEntities(context);
        }

        public override Type InvokeAfter => typeof(CommunitySeeder);

        private void UpdateEntities(SqliteContext context)
        {
            List<IncidentTask> seedEntities = GetSeedEntities();

            foreach (var item in seedEntities)
            {
                CreateOrUpdateFromSeedEntity(context, item);
            }

            AddInitialEntities(context);
        }

        private void CreateOrUpdateFromSeedEntity(SqliteContext context, IncidentTask seedData)
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

        private void AddInitialEntities(SqliteContext context)
        {
            string filename = Configuration["IncidentTaskInitializationFilename"];
            if (string.IsNullOrEmpty(filename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "IncidentTasks.json");
            }
            SeedFromFile(context, filename);
        }

        /// <summary>
        /// Creates entities from a (json) file
        /// </summary>
        private void SeedFromFile(SqliteContext context, string filename)
        {
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                SeedFromJson(context, json);
            }
        }

        private void SeedFromJson(SqliteContext context, string json)
        {
            var entities = JsonConvert.DeserializeObject<List<IncidentTask>>(json);
            if (entities != null)
            {
                entities.ForEach(item => AddData(context, item));
            }
        }

        /// <summary>
        /// Adds an entity to the system, only if it does not exist.
        /// </summary>
        private void AddData(SqliteContext context, IncidentTask seedItem)
        {
            var entity = context.IncidentTasks.FirstOrDefault(x => x.TaskNumber == seedItem.TaskNumber);
            if (entity != null)
            {
                // Entity has already been seeded; abort
                return;
            }

            // avoid null pointer errors by using "?" operator (safe navigation operator)
            var communityName = seedItem.Community?.Name;
            var regionalDistrictName = seedItem.RegionalDistrict?.Name;
            var regionName = seedItem.Region?.Name;

            entity = new IncidentTask()
            {
                Id = seedItem.Id,
                Details = seedItem.Details,
                Active = seedItem.Active,
            };

            // get the community (if any).
            if (communityName != null)
            {
                entity.Community = context.Communities.FirstOrDefault(c => c.Name == communityName);
            }
            // get the regional district (if any).
            else if (regionalDistrictName != null)
            {
                entity.RegionalDistrict = context.RegionalDistricts.FirstOrDefault(rd => rd.Name == regionalDistrictName);
            }
            // get the region (if any).
            else if (regionName != null)
            {
                entity.Region = context.Regions.FirstOrDefault(reg => reg.Name == regionName);
            }

            context.IncidentTasks.Add(entity);
            context.SaveChanges();
        }

        private List<IncidentTask> GetSeedEntities()
        {
            List<IncidentTask> entities = new List<IncidentTask>(GetDefaultEntities());

            if (IsProductionEnvironment)
            {
                entities.AddRange(GetProdEntities());
            }
            else
            {
                entities.AddRange(GetDevEntities());
            }

            return entities;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<IncidentTask> GetDefaultEntities()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<IncidentTask> GetDevEntities()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<IncidentTask> GetTestEntities()
        {
            return new List<IncidentTask>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<IncidentTask> GetProdEntities()
        {
            return new List<IncidentTask>();
        }
    }
}
