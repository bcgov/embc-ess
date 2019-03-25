using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class CountryExtensions
    {
        public static void AddCountry(this EmbcDbContext context, Country Country)
        {
            // create a new Country.           
            context.Countries.Add(Country);
            context.SaveChanges();
        }

        public static void UpdateCountry(this EmbcDbContext context, Country Country)
        {
            Country _Country = context.Countries.FirstOrDefault<Country>(x => x.Id == Country.Id);
            _Country.Name = Country.Name;
            context.Countries.Update(_Country);
            context.SaveChanges();
        }

        public static List<Country> GetCountries(this EmbcDbContext context)
        {
            List<Country> Countries =
                context.Countries.ToList<Country>();
            return Countries;
        }

        /// <summary>
        /// Returns a specific Country
        /// </summary>
        /// <param name="name">The name of the Country</param>
        /// <returns>The Country, or null if it does not exist.</returns>
        public static Country GetCountryByName(this EmbcDbContext context, string name)
        {
            Country Country = context.Countries.FirstOrDefault(x => x.Name == name);
            return Country;
        }



        /// <summary>
        /// Create Countries from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CountryJsonPath"></param>
        public static void AddInitialCountriesFromFile(this EmbcDbContext context, string CountryJsonPath)
        {
            if (!string.IsNullOrEmpty(CountryJsonPath) && File.Exists(CountryJsonPath))
            {
                string CountryJson = File.ReadAllText(CountryJsonPath);
                context.AddInitialCountries(CountryJson);
            }
        }

        private static void AddInitialCountries(this EmbcDbContext context, string CountryJson)
        {
            List<Country> Countries = JsonConvert.DeserializeObject<List<Country>>(CountryJson);

            if (Countries != null)
            {
                context.AddInitialCountries(Countries);
            }
        }

        private static void AddInitialCountries(this EmbcDbContext context, List<Country> Countries)
        {
            Countries.ForEach(context.AddInitialCountry);
        }

        /// <summary>
        /// Adds a Country to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialCountry(this EmbcDbContext context, Country initialCountry)
        {
            Country Country = context.GetCountryByName(initialCountry.Name);
            if (Country != null)
            {
                return;
            }
            
            Country = new Country
            ()
            {
                Id = initialCountry.Id,
                Name = initialCountry.Name,
                Active = true
            };

            context.AddCountry(Country);
        }


        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CountryInfo"></param>
        public static void UpdateSeedCountryInfo(this EmbcDbContext context, Country CountryInfo)
        {
            Country Country = context.GetCountryByName(CountryInfo.Name);
            if (Country == null)
            {
                context.AddCountry(CountryInfo);
            }
            else
            {
                Country.Name = CountryInfo.Name;
                Country.Active = CountryInfo.Active;                
                context.UpdateCountry(Country);
            }
        }
    }
}
