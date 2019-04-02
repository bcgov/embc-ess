using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Embc.Public.Seeders
{
    public class OrganizationSeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { "Development", "Staging", "Test" };

        public OrganizationSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateData(context);
        }

        public override int InvokeOrder => 7;

        private void UpdateData(EmbcDbContext context)
        {
            List<Organization> seedEntities = GetSeedData();

            foreach (var item in seedEntities)
            {
                CreateOrUpdateFromSeedEntity(context, item);
            }

            AddInitialData(context);
        }

        private void CreateOrUpdateFromSeedEntity(EmbcDbContext context, Organization seedData)
        {
            var existing = context.Organizations.FirstOrDefault(x => x.BceidAccountNumber == seedData.BceidAccountNumber);
            if (existing == null)
            {
                context.Organizations.Add(seedData);
            }
            else
            {
                existing.BceidAccountNumber = seedData.BceidAccountNumber;
                existing.Name = seedData.Name;
                existing.Externaluseridentifier = seedData.Externaluseridentifier;
                existing.Active = seedData.Active;
                existing.Community = seedData.Community;
                existing.RegionalDistrict = seedData.RegionalDistrict;
                existing.Region = seedData.Region;
                context.Organizations.Update(existing);
            }
            context.SaveChanges();
        }

        private void AddInitialData(EmbcDbContext context)
        {
            string filename = Configuration["OrganizationInitializationFilename"];
            if (string.IsNullOrEmpty(filename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "Organizations.json");
            }
            context.AddInitialOrganizationsFromFile(filename);
        }

        private List<Organization> GetSeedData()
        {
            List<Organization> entities = new List<Organization>(GetDefaultData());

            if (IsProductionEnvironment)
            {
                entities.AddRange(GetProdData());
            }
            else
            {
                entities.AddRange(GetDevData());
            }

            return entities;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<Organization> GetDefaultData()
        {
            return new List<Organization>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<Organization> GetDevData()
        {
            return new List<Organization>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<Organization> GetTestData()
        {
            return new List<Organization>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<Organization> GetProdData()
        {
            return new List<Organization>();
        }
    }
}
