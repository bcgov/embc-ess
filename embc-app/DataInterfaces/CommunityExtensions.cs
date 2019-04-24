using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class CommunityExtensions
    {
        public static void AddCommunity(this EmbcDbContext context, Community Community)
        {
            // create a new Community.
            context.Communities.Add(Community);
            context.SaveChanges();
        }

        public static void UpdateCommunity(this EmbcDbContext context, Community Community)
        {
            Community _Community = context.Communities.FirstOrDefault<Community>(x => x.Id == Community.Id);
            _Community.Name = Community.Name;
            context.Communities.Update(_Community);
            context.SaveChanges();
        }

        /// <summary>
        /// Returns a specific Community
        /// </summary>
        /// <param name="name">The name of the Community</param>
        /// <returns>The Community, or null if it does not exist.</returns>
        public static Community GetCommunityByName(this EmbcDbContext context, string name)
        {
            Community Community = context.Communities.FirstOrDefault(x => x.Name == name);
            return Community;
        }

        /// <summary>
        /// Create Communities from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CommunityJsonPath"></param>
        public static void AddInitialCommunitiesFromFile(this EmbcDbContext context, string CommunityJsonPath)
        {
            if (!string.IsNullOrEmpty(CommunityJsonPath) && File.Exists(CommunityJsonPath))
            {
                string CommunityJson = File.ReadAllText(CommunityJsonPath);
                context.AddInitialCommunities(CommunityJson);
            }
        }

        private static void AddInitialCommunities(this EmbcDbContext context, string CommunityJson)
        {
            List<Community> Communities = JsonConvert.DeserializeObject<List<Community>>(CommunityJson);

            if (Communities != null)
            {
                context.AddInitialCommunities(Communities);
            }
        }

        private static void AddInitialCommunities(this EmbcDbContext context, List<Community> Communities)
        {
            Communities.ForEach(context.AddInitialCommunity);
        }

        /// <summary>
        /// Adds a Community to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialCommunity(this EmbcDbContext context, Community initialCommunity)
        {
            Community Community = context.GetCommunityByName(initialCommunity.Name);
            if (Community != null)
            {
                return;
            }

            // get the region.

            Region region = null;

            if (initialCommunity.Region != null)
            {
                region = context.GetRegionByName(initialCommunity.Region.Name);
            }

            Community = new Community
            ()
            {
                //Id = initialCommunity.Id,
                Name = initialCommunity.Name,
                Active = true,
                Region = region
            };

            context.AddCommunity(Community);
        }

        /// <summary>
        /// Update Community
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CommunityInfo"></param>
        public static void UpdateSeedCommunityInfo(this EmbcDbContext context, Community CommunityInfo)
        {
            Community Community = context.GetCommunityByName(CommunityInfo.Name);
            if (Community == null)
            {
                context.AddCommunity(CommunityInfo);
            }
            else
            {
                Community.Name = CommunityInfo.Name;
                Community.Active = CommunityInfo.Active;
                Community.Region = CommunityInfo.Region;
                context.UpdateCommunity(Community);
            }
        }
    }
}
