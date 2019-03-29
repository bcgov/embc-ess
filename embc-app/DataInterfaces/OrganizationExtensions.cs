using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class OrganizationExtensions
    {
        public static void AddOrganization(this EmbcDbContext context, Organization organization)
        {
            context.Organizations.Add(organization);
            context.SaveChanges();
        }

        public static Organization GetOrganizationByBceidAccountNumber(this EmbcDbContext context, string bceidAccountNumber)
        {
            var item = context.Organizations.FirstOrDefault(x => x.BceidAccountNumber == bceidAccountNumber);
            return item;
        }

        public static void AddInitialOrganizationsFromFile(this EmbcDbContext context, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                context.AddOrganizations(json);
            }
        }

        private static void AddOrganizations(this EmbcDbContext context, string json)
        {
            List<Organization> items = JsonConvert.DeserializeObject<List<Organization>>(json);

            if (items != null)
            {
                context.AddInitialOrganizations(items);
            }
        }

        private static void AddInitialOrganizations(this EmbcDbContext context, List<Organization> items)
        {
            items.ForEach(context.AddInitialOrganization);
        }

        private static void AddInitialOrganization(this EmbcDbContext context, Organization item)
        {
            var i = context.GetOrganizationByBceidAccountNumber(item.BceidAccountNumber);
            if (i != null)
            {
                return;
            }

            Region region = null;

            if (item.Region != null)
            {
                region = context.GetRegionByName(i.Region.Name);
            }

            RegionalDistrict regionalDistrict = null;

            if (item.RegionalDistrict != null)
            {
                regionalDistrict = context.GetRegionalDistrictByName(item.RegionalDistrict.Name);
            }

            Community community = null;

            if (item.Community != null)
            {
                community = context.GetCommunityByName(item.Community.Name);
            }

            item = new Organization
            {
                BceidAccountNumber = item.BceidAccountNumber,
                Active = true,
                RegionId = region?.Id,
                RegionalDistrictId = regionalDistrict?.Id,
                CommunityId = community?.Id,
                Externaluseridentifier = item.Externaluseridentifier,
                Name = item.Name
            };

            context.AddOrganization(item);
        }
    }
}
