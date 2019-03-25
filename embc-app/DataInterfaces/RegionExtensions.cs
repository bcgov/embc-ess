using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class RegionExtensions
    {
        public static void AddRegion(this EmbcDbContext context, Region Region)
        {
            // create a new Region.           
            context.Regions.Add(Region);
            context.SaveChanges();
        }

        public static void UpdateRegion(this EmbcDbContext context, Region Region)
        {
            Region _Region = context.Regions.FirstOrDefault<Region>(x => x.Id == Region.Id);
            _Region.Name = Region.Name;
            context.Regions.Update(_Region);
            context.SaveChanges();
        }

        public static List<Region> GetRegions(this EmbcDbContext context)
        {
            List<Region> Regions =
                context.Regions.ToList<Region>();
            return Regions;
        }

        /// <summary>
        /// Returns a specific Region
        /// </summary>
        /// <param name="name">The name of the Region</param>
        /// <returns>The Region, or null if it does not exist.</returns>
        public static Region GetRegionByName(this EmbcDbContext context, string name)
        {
            Region Region = context.Regions.FirstOrDefault(x => x.Name == name);
            return Region;
        }



        /// <summary>
        /// Create Regions from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RegionJsonPath"></param>
        public static void AddInitialRegionsFromFile(this EmbcDbContext context, string RegionJsonPath)
        {
            if (!string.IsNullOrEmpty(RegionJsonPath) && File.Exists(RegionJsonPath))
            {
                string RegionJson = File.ReadAllText(RegionJsonPath);
                context.AddInitialRegions(RegionJson);
            }
        }

        private static void AddInitialRegions(this EmbcDbContext context, string RegionJson)
        {
            List<Region> Regions = JsonConvert.DeserializeObject<List<Region>>(RegionJson);

            if (Regions != null)
            {
                context.AddInitialRegions(Regions);
            }
        }

        private static void AddInitialRegions(this EmbcDbContext context, List<Region> Regions)
        {
            Regions.ForEach(context.AddInitialRegion);
        }

        /// <summary>
        /// Adds a Region to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialRegion(this EmbcDbContext context, Region initialRegion)
        {
            Region Region = context.GetRegionByName(initialRegion.Name);
            if (Region != null)
            {
                return;
            }

            Region = new Region
            ()
            {
                Id = initialRegion.Id,
                Name = initialRegion.Name,
                Active = true
            };

            context.AddRegion(Region);
        }


        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="context"></param>
        /// <param name="regionInfo"></param>
        public static void UpdateSeedRegionInfo(this EmbcDbContext context, Region RegionInfo)
        {
            Region Region = context.GetRegionByName(RegionInfo.Name);
            if (Region == null)
            {
                context.AddRegion(RegionInfo);
            }
            else
            {
                Region.Name = RegionInfo.Name;
                Region.Active = RegionInfo.Active;
                context.UpdateRegion(Region);
            }
        }
    }
}
