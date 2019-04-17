using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class SeedDataLoader
    {
        private readonly ILogger logger;

        public SeedDataLoader(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(typeof(SeedDataLoader));
        }

        public T GetSeedData<T>(string seedDataFileName)
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
                logger.LogError($"{seedDataFileName}.json file structure error!{Environment.NewLine}{ex.Message}");
                return default(T);
            }
        }
    }
}
