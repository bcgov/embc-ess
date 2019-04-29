using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class RegionExtensions
    {
        public static void AddRegion(this EmbcDbContext context, Region newRegion)
        {
            // create a new Region.
            context.Regions.Add(newRegion);
            context.SaveChanges();
        }

        public static void UpdateRegion(this EmbcDbContext context, Region udpatedRegion)
        {
            var region = context.Regions.FirstOrDefault<Region>(x => x.Name == udpatedRegion.Name);
            region.Active = udpatedRegion.Active;
            context.Regions.Update(region);
            context.SaveChanges();
        }

        /// <summary>
        /// Returns a specific Region
        /// </summary>
        /// <param name="name">The name of the Region</param>
        /// <returns>The Region, or null if it does not exist.</returns>
        public static Region GetRegionByName(this EmbcDbContext context, string name)
        {
            var region = context.Regions.FirstOrDefault(x => x.Name == name);
            return region;
        }

        /// <summary>
        /// Create Regions from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="regionJsonPath"></param>
        public static void AddInitialRegionsFromFile(this EmbcDbContext context, string regionJsonPath)
        {
            if (!string.IsNullOrEmpty(regionJsonPath) && File.Exists(regionJsonPath))
            {
                var regionJson = File.ReadAllText(regionJsonPath);
                context.AddInitialRegions(regionJson);
            }
        }

        private static void AddInitialRegions(this EmbcDbContext context, string regionJson)
        {
            var regions = JsonConvert.DeserializeObject<List<Region>>(regionJson);

            if (regions != null)
            {
                context.AddInitialRegions(regions);
            }
        }

        private static void AddInitialRegions(this EmbcDbContext context, List<Region> regions)
        {
            regions.ForEach(context.AddInitialRegion);
        }

        /// <summary>
        /// Adds a Region to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialRegion(this EmbcDbContext context, Region initialRegion)
        {
            var region = context.GetRegionByName(initialRegion.Name);
            if (region != null)
            {
                return;
            }

            region = new Region
            ()
            {
                Name = initialRegion.Name,
                Active = true
            };

            context.AddRegion(region);
        }

        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="context"></param>
        /// <param name="regionInfo"></param>
        public static void UpdateSeedRegionInfo(this EmbcDbContext context, Region regionInfo)
        {
            var region = context.GetRegionByName(regionInfo.Name);
            if (region == null)
            {
                context.AddRegion(regionInfo);
            }
            else
            {
                region.Name = regionInfo.Name;
                region.Active = regionInfo.Active;
                context.UpdateRegion(region);
            }
        }
    }
}
