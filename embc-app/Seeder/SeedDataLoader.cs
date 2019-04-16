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
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream($"Gov.Jag.Embc.Public.Seeder.SeedData.{seedDataFileName}.json"))
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{seedDataFileName}.json file structure error!{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
