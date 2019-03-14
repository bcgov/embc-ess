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
    public class CountrySeeder : Seeder<SqliteContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public CountrySeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(SqliteContext context)
        {
            UpdateCountries(context);
        }

        private void UpdateCountries(SqliteContext context)
        {
            List<Country> seedCountries = GetSeedCountries();

            foreach (Country Country in seedCountries)
            {
                context.UpdateSeedCountryInfo(Country);
            }

            AddInitialCountries(context);
        }

        private void AddInitialCountries(SqliteContext context)
        {
            string CountryInitializationFilename = Configuration["CountryInitializationFilename"];
            if (string.IsNullOrEmpty(CountryInitializationFilename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                CountryInitializationFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "Countries.json");
            }
            context.AddInitialCountriesFromFile(CountryInitializationFilename);
        }

        private List<Country> GetSeedCountries()
        {
            List<Country> jurisdictions = new List<Country>(GetDefaultCountries());

            if (IsProductionEnvironment)
            {
                jurisdictions.AddRange(GetProdCountries());
            }
            else
            {
                jurisdictions.AddRange(GetDevCountries());
            }

            return jurisdictions;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<Country> GetDefaultCountries()
        {
            return new List<Country>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<Country> GetDevCountries()
        {
            return new List<Country>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<Country> GetTestCountries()
        {
            return new List<Country>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<Country> GetProdCountries()
        {
            return new List<Country>();
        }
    }
}
