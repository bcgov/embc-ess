using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.IO;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Sqlite.Models;

namespace Gov.Embc.Public.Seeders
{
    public class JurisdictionSeeder : Seeder<SqliteContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public JurisdictionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory  loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(SqliteContext context)
        {
            UpdateRegions(context);            
        }

        private void UpdateRegions(SqliteContext context)
        {
            List<Region> seedRegions = GetSeedRegions();

            foreach (Region region in seedRegions)
            {
                context.UpdateSeedRegionInfo(region);                
            }

            AddInitialRegions(context);            
        }

        private void AddInitialRegions(SqliteContext context)
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
            List<Region> jurisdictions = new List<Region>(GetDefaultRegions());
                
            if (IsProductionEnvironment)
            {
                jurisdictions.AddRange(GetProdRegions());
            }
            else
            {
                jurisdictions.AddRange(GetDevRegions());
            }

            return jurisdictions;
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
