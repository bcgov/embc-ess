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
    public class CommunitySeeder : Seeder<SqliteContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public CommunitySeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(SqliteContext context)
        {
            UpdateCommunities(context);
        }

        public override Type InvokeAfter => typeof(RegionalDistrictSeeder);

        private void UpdateCommunities(SqliteContext context)
        {
            List<Community> seedCommunities = GetSeedCommunities();

            foreach (Community Community in seedCommunities)
            {
                context.UpdateSeedCommunityInfo(Community);
            }

            AddInitialCommunities(context);
        }

        private void AddInitialCommunities(SqliteContext context)
        {
            string CommunityInitializationFilename = Configuration["CommunityInitializationFilename"];
            if (string.IsNullOrEmpty(CommunityInitializationFilename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                CommunityInitializationFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "Communities.json");
            }
            context.AddInitialCommunitiesFromFile(CommunityInitializationFilename);
        }

        private List<Community> GetSeedCommunities()
        {
            List<Community> communities = new List<Community>(GetDefaultCommunities());

            if (IsProductionEnvironment)
            {
                communities.AddRange(GetProdCommunities());
            }
            else
            {
                communities.AddRange(GetDevCommunities());
            }

            return communities;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<Community> GetDefaultCommunities()
        {
            return new List<Community>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<Community> GetDevCommunities()
        {
            return new List<Community>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<Community> GetTestCommunities()
        {
            return new List<Community>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<Community> GetProdCommunities()
        {
            return new List<Community>();
        }
    }
}
