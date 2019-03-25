using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gov.Embc.Public.Seeders
{
    public class RegionSeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public RegionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateRegions(context);
        }

        private void UpdateRegions(EmbcDbContext context)
        {
            List<Region> seedRegions = GetSeedRegions();

            foreach (Region region in seedRegions)
            {
                context.UpdateSeedRegionInfo(region);
            }

            AddInitialRegions(context);
        }

        private void AddInitialRegions(EmbcDbContext context)
        {
            string regionInitializationFilename = Configuration["RegionInitializationFilename"];
            if (string.IsNullOrEmpty(regionInitializationFilename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                regionInitializationFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "Regions.json");
            }
            context.AddInitialRegionsFromFile(regionInitializationFilename);
        }

        private List<Region> GetSeedRegions()
        {
            List<Region> regions = new List<Region>(GetDefaultRegions());

            if (IsProductionEnvironment)
            {
                regions.AddRange(GetProdRegions());
            }
            else
            {
                regions.AddRange(GetDevRegions());
            }

            return regions;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<Region> GetDefaultRegions()
        {
            return new List<Region>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<Region> GetDevRegions()
        {
            return new List<Region>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<Region> GetTestRegions()
        {
            return new List<Region>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<Region> GetProdRegions()
        {
            return new List<Region>();
        }
    }
}
