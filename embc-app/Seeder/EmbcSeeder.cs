using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class EmbcSeeder
    {
        private readonly ILogger logger;
        private readonly ISeederRepository seederRepository;
        private readonly IHostingEnvironment environment;
        private readonly SeedDataLoader seedDataLoader;

        public EmbcSeeder(ILoggerFactory loggerFactory, ISeederRepository seederRepository, IHostingEnvironment environment, SeedDataLoader seedDataLoader)
        {
            logger = loggerFactory.CreateLogger(typeof(EmbcSeeder));
            this.seederRepository = seederRepository;
            this.environment = environment;
            this.seedDataLoader = seedDataLoader;
        }

        public void SeedData()
        {
            SeedRegions();
            SeedCommunities();
            SeedIncidentTasks();
            SeedFamilyRelationshipType();
            SeedCountries();
            SeedOrganziations();
            SeedVolunteers();
        }

        private void SeedRegions()
        {
            var regions = seedDataLoader.GetSeedData<List<Region>>("Regions");

            seederRepository.AddOrUpdateRegions(regions);
        }

        private void SeedCommunities()
        {
            var communities = seedDataLoader.GetSeedData<List<Community>>("Communities");

            var regions = seederRepository.GetRegions().ToList();

            communities.ToList().ForEach(c =>
                {
                    c.RegionName = c.Region.Name;
                    c.Region = null;
                }
            );

            seederRepository.AddOrUpdateCommunities(communities
                .GroupBy(c => new { c.Name, c.RegionName })
                .Select(g => g.First())
                .ToList());
        }

        private void SeedIncidentTasks()
        {
            string[] targetEnvironments = { "Development", "Staging", "Test" };
            if (!targetEnvironments.Contains(environment.EnvironmentName))
            {
                return;
            }

            var incidentTasks = seedDataLoader.GetSeedData<List<IncidentTask>>("IncidentTasks");
            var regions = seederRepository.GetRegions() ?? new List<Region>();
            var incidentTaskCommunities = incidentTasks.Where(it => it.Community != null).Select(s => s.Community).ToList() ?? new List<Community>();
            var communties = seederRepository.GetCommunities().Where(c => incidentTaskCommunities.Exists(it => it.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)));

            incidentTasks.ToList().ForEach(it =>
                {
                    it.RegionName = it.Region?.Name;
                    it.Region = null;
                    it.CommunityId = communties.SingleOrDefault(c => c.Name.Equals(it.Community?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    it.Community = null;
                }
            );

            seederRepository.AddOrUpdateIncidentTasks(incidentTasks
                .GroupBy(c => c.TaskNumber)
                .Select(g => g.First())
                .ToList());
        }

        private void SeedFamilyRelationshipType()
        {
            var relationshipTypes = seedDataLoader.GetSeedData<List<FamilyRelationshipType>>("FamilyRelationshipTypes");

            seederRepository.AddOrUpdateFamilyRelationshipTypes(relationshipTypes.GroupBy(r => r.Code).Select(g => g.First()).ToList());
        }
        private void SeedCountries()
        {
            var countries = seedDataLoader.GetSeedData<List<Country>>("Countries");

            seederRepository.AddOrUpdateCountries(countries.GroupBy(c => c.Name).Select(g => g.First()).ToList());
        }
        private void SeedOrganziations()
        {
            string[] targetEnvironments = { "Development", "Staging", "Test" };
            if (!targetEnvironments.Contains(environment.EnvironmentName))
            {
                return;
            }

            var organizations = seedDataLoader.GetSeedData<List<Organization>>("Organizations");
            var regions = seederRepository.GetRegions() ?? new List<Region>();
            var organizationCommunities = organizations.Where(it => it.Community != null).Select(s => s.Community).ToList() ?? new List<Community>();
            var communties = seederRepository.GetCommunities().Where(c => organizationCommunities.Exists(it => it.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)));

            organizations.ToList().ForEach(o =>
                {
                    o.RegionName = o.Region?.Name;
                    o.Region = null;
                    o.CommunityId = communties.SingleOrDefault(c => c.Name.Equals(o.Community?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    o.Community = null;
                }
            );

            seederRepository.AddOrUpdateOrganizations(organizations.GroupBy(o => o.BCeIDBusinessGuid).Select(g => g.First()).ToList());
        }
        private void SeedVolunteers()
        {
            string[] targetEnvironments = { "Development", "Staging", "Test" };
            if (!targetEnvironments.Contains(environment.EnvironmentName))
            {
                return;
            }

            var volunteers = seedDataLoader.GetSeedData<List<Volunteer>>("Volunteers");
            var organizations = seederRepository.GetOrganizations().ToList();

            volunteers.ForEach(v =>
            {
                v.OrganizationId = organizations
                    .Single(o => o.BCeIDBusinessGuid.Equals(v.Organization.BCeIDBusinessGuid, StringComparison.OrdinalIgnoreCase)).Id;
                v.Organization = null;
            });

            seederRepository.AddOrUpdateVolunteers(volunteers.GroupBy(v => v.BceidAccountNumber).Select(g => g.First()).ToList());
        }
    }
}
