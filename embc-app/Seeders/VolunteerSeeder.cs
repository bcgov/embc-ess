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
    public class VolunteerSeeder : Seeder<EmbcDbContext>
    {
        private readonly string[] _profileTriggers = { "Development", "Staging", "Test" };

        public VolunteerSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(EmbcDbContext context)
        {
            UpdateData(context);
        }

        public override int InvokeOrder => 8;

        private void UpdateData(EmbcDbContext context)
        {
            List<Volunteer> seedEntities = GetSeedData();

            foreach (var item in seedEntities)
            {
                CreateOrUpdateFromSeedEntity(context, item);
            }

            AddInitialData(context);
        }

        private void CreateOrUpdateFromSeedEntity(EmbcDbContext context, Volunteer seedData)
        {
            var existing = context.People.Where(x => x is Volunteer).Cast<Volunteer>().FirstOrDefault(x => x.BceidAccountNumber == seedData.BceidAccountNumber);
            if (existing == null)
            {
                context.People.Add(seedData);
            }
            else
            {
                existing.BceidAccountNumber = seedData.BceidAccountNumber;
                existing.Name = seedData.Name;
                existing.Externaluseridentifier = seedData.Externaluseridentifier;
                existing.Active = seedData.Active;
                existing.CanAccessRestrictedFiles = seedData.CanAccessRestrictedFiles;
                existing.Email = seedData.Email;
                existing.FirstName = seedData.FirstName;
                existing.OrganizationId = seedData.OrganizationId;
                existing.IsAdministrator = seedData.IsAdministrator;
                existing.IsPrimaryContact = seedData.IsPrimaryContact;
                context.People.Update(existing);
            }
            context.SaveChanges();
        }

        private void AddInitialData(EmbcDbContext context)
        {
            string filename = Configuration["OrganizationInitializationFilename"];
            if (string.IsNullOrEmpty(filename))
            {
                // default to sample data, which is stored in the "SeedData" directory.
                filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData" + Path.DirectorySeparatorChar + "volunteers.json");
            }
            context.AddInitialVolunteersFromFile(filename);
        }

        private List<Volunteer> GetSeedData()
        {
            List<Volunteer> entities = new List<Volunteer>(GetDefaultData());

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
        private List<Volunteer> GetDefaultData()
        {
            return new List<Volunteer>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Development environment.
        /// </summary>
        private List<Volunteer> GetDevData()
        {
            return new List<Volunteer>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Test environment.
        /// </summary>
        private List<Volunteer> GetTestData()
        {
            return new List<Volunteer>();
        }

        /// <summary>
        /// Returns a list of jurisdictions to be populated in the Production environment.
        /// </summary>
        private List<Volunteer> GetProdData()
        {
            return new List<Volunteer>();
        }
    }
}
