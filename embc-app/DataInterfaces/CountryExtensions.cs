using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class CountryExtensions
    {
        public static void AddCountry(this EmbcDbContext context, Country newCountry)
        {
            // create a new Country.
            context.Countries.Add(newCountry);
            context.SaveChanges();
        }

        public static void UpdateCountry(this EmbcDbContext context, Country updatedCountry)
        {
            var country = context.Countries.FirstOrDefault(x => x.CountryCode.Equals(updatedCountry.CountryCode, StringComparison.OrdinalIgnoreCase));
            country.Name = updatedCountry.Name;
            context.Countries.Update(country);
            context.SaveChanges();
        }

        /// <summary>
        /// Returns a specific Country
        /// </summary>
        /// <param name="name">The name of the Country</param>
        /// <returns>The Country, or null if it does not exist.</returns>
        public static Country GetCountryByName(this EmbcDbContext context, string name)
        {
            var country = context.Countries.FirstOrDefault(x => x.Name == name);
            return country;
        }

        /// <summary>
        /// Create Countries from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="countryJsonPath"></param>
        public static void AddInitialCountriesFromFile(this EmbcDbContext context, string countryJsonPath)
        {
            if (!string.IsNullOrEmpty(countryJsonPath) && File.Exists(countryJsonPath))
            {
                string countryJson = File.ReadAllText(countryJsonPath);
                context.AddInitialCountries(countryJson);
            }
        }

        private static void AddInitialCountries(this EmbcDbContext context, string countryJson)
        {
            var countries = JsonConvert.DeserializeObject<List<Country>>(countryJson);

            if (countries != null)
            {
                context.AddInitialCountries(countries);
            }
        }

        private static void AddInitialCountries(this EmbcDbContext context, List<Country> countries)
        {
            countries.ForEach(context.AddInitialCountry);
        }

        /// <summary>
        /// Adds a Country to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialCountry(this EmbcDbContext context, Country initialCountry)
        {
            var country = context.GetCountryByName(initialCountry.Name);
            if (country != null)
            {
                return;
            }

            country = new Country
            ()
            {
                CountryCode = initialCountry.CountryCode,
                Name = initialCountry.Name,
                Active = true
            };

            context.AddCountry(country);
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="context"></param>
        /// <param name="updatedCountry"></param>
        public static void UpdateSeedCountryInfo(this EmbcDbContext context, Country updatedCountry)
        {
            var country = context.GetCountryByName(updatedCountry.Name);
            if (country == null)
            {
                context.AddCountry(updatedCountry);
            }
            else
            {
                country.Name = updatedCountry.Name;
                country.Active = updatedCountry.Active;
                context.UpdateCountry(country);
            }
        }
    }
}
