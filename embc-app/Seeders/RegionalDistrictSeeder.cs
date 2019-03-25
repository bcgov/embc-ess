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
    public class RegionalDistrictSeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public RegionalDistrictSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateRegionalDistricts(context);
        }

        public override int InvokeOrder => 1;

        private void UpdateRegionalDistricts(EmbcDbContext context)
        {
            List<RegionalDistrict> seedRegionalDistricts = GetSeedRegionalDistricts();

            foreach (RegionalDistrict RegionalDistrict in seedRegionalDistricts)
            {
                context.UpdateSeedRegionalDistrictInfo(RegionalDistrict);
            }

            AddInitialRegionalDistricts(context);
        }

        private void AddInitialRegionalDistricts(EmbcDbContext context)
        {
            string RegionalDistrictInitializationFilename = Configuration["RegionalDistrictInitializationFilename"];
            if (string.IsNullOrEmpty(RegionalDistrictInitializationFilename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                RegionalDistrictInitializationFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "RegionalDistricts.json");
            }
            context.AddInitialRegionalDistrictsFromFile(RegionalDistrictInitializationFilename);
        }

        private List<RegionalDistrict> GetSeedRegionalDistricts()
        {
            List<RegionalDistrict> regionalDistricts = new List<RegionalDistrict>(GetDefaultRegionalDistricts());

            if (IsProductionEnvironment)
            {
                regionalDistricts.AddRange(GetProdRegionalDistricts());
            }
            else
            {
                regionalDistricts.AddRange(GetDevRegionalDistricts());
            }

            return regionalDistricts;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<RegionalDistrict> GetDefaultRegionalDistricts()
        {
            return new List<RegionalDistrict>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<RegionalDistrict> GetDevRegionalDistricts()
        {
            return new List<RegionalDistrict>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<RegionalDistrict> GetTestRegionalDistricts()
        {
            return new List<RegionalDistrict>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<RegionalDistrict> GetProdRegionalDistricts()
        {
            return new List<RegionalDistrict>();
        }
    }
}
