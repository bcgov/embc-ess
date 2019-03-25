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
    public class CountrySeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public CountrySeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateCountries(context);
        }

        public override int InvokeOrder => 6;

        private void UpdateCountries(EmbcDbContext context)
        {
            List<Country> seedCountries = GetSeedCountries();

            foreach (Country Country in seedCountries)
            {
                context.UpdateSeedCountryInfo(Country);
            }

            AddInitialCountries(context);
        }

        private void AddInitialCountries(EmbcDbContext context)
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
            List<Country> countries = new List<Country>(GetDefaultCountries());

            if (IsProductionEnvironment)
            {
                countries.AddRange(GetProdCountries());
            }
            else
            {
                countries.AddRange(GetDevCountries());
            }

            return countries;
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
