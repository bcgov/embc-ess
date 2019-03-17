using Gov.Jag.Embc.Public.Sqlite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class CommunityExtensions
    {
        public static void AddCommunity(this SqliteContext context, Community Community)
        {
            // create a new Community.           
            context.Communities.Add(Community);
            context.SaveChanges();
        }

        public static void UpdateCommunity(this SqliteContext context, Community Community)
        {
            Community _Community = context.Communities.FirstOrDefault<Community>(x => x.Id == Community.Id);
            _Community.Name = Community.Name;
            context.Communities.Update(_Community);
            context.SaveChanges();
        }

        public static List<Community> GetCommunities(this SqliteContext context)
        {
            List<Community> Communities =
                context.Communities.ToList<Community>();
            return Communities;
        }

        /// <summary>
        /// Returns a specific Community
        /// </summary>
        /// <param name="name">The name of the Community</param>
        /// <returns>The Community, or null if it does not exist.</returns>
        public static Community GetCommunityByName(this SqliteContext context, string name)
        {
            Community Community = context.Communities.FirstOrDefault(x => x.Name == name);
            return Community;
        }



        /// <summary>
        /// Create Communities from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CommunityJsonPath"></param>
        public static void AddInitialCommunitiesFromFile(this SqliteContext context, string CommunityJsonPath)
        {
            if (!string.IsNullOrEmpty(CommunityJsonPath) && File.Exists(CommunityJsonPath))
            {
                string CommunityJson = File.ReadAllText(CommunityJsonPath);
                context.AddInitialCommunities(CommunityJson);
            }
        }

        private static void AddInitialCommunities(this SqliteContext context, string CommunityJson)
        {
            List<Community> Communities = JsonConvert.DeserializeObject<List<Community>>(CommunityJson);

            if (Communities != null)
            {
                context.AddInitialCommunities(Communities);
            }
        }

        private static void AddInitialCommunities(this SqliteContext context, List<Community> Communities)
        {
            Communities.ForEach(context.AddInitialCommunity);
        }

        /// <summary>
        /// Adds a Community to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialCommunity(this SqliteContext context, Community initialCommunity)
        {
            Community Community = context.GetCommunityByName(initialCommunity.Name);
            if (Community != null)
            {
                return;
            }

            // get the region.

            RegionalDistrict regionalDistrict = null;

            if (initialCommunity.RegionalDistrict != null)
            {
                regionalDistrict = context.GetRegionalDistrictByName(initialCommunity.RegionalDistrict.Name);
            }


            Community = new Community
            ()
            {
                //Id = initialCommunity.Id,
                Name = initialCommunity.Name,
                Active = true,
                RegionalDistrict = regionalDistrict
            };            

            context.AddCommunity(Community);
        }


        /// <summary>
        /// Update Community
        /// </summary>
        /// <param name="context"></param>
        /// <param name="CommunityInfo"></param>
        public static void UpdateSeedCommunityInfo(this SqliteContext context, Community CommunityInfo)
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
                Community.RegionalDistrict = CommunityInfo.RegionalDistrict;
                context.UpdateCommunity(Community);
            }
        }
    }
}
