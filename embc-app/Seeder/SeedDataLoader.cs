using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class SeedDataLoader
    {
        public static T GetSeedData<T>(string seedDataFileName)
        {
            try
            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seeder/SeedData" + Path.DirectorySeparatorChar + $"{seedDataFileName}.json");
                string json = File.ReadAllText(fileName);
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"SeedDataLoader json file structure error!{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
