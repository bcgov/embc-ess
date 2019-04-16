using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class EmbcSeeder
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ISeederRepository _seederRepository;

        public EmbcSeeder(ILoggerFactory loggerFactory, ISeederRepository seederRepository)
        {
            _loggerFactory = loggerFactory;
            _seederRepository = seederRepository;
        }

        public void SeedData()
        {
            SeedRegions();
            SeedRegionalDistricts();
            SeedCommunities();
            SeedIncidentTasks();
            SeedFamilyRelationshipType();
            SeedCountries();
            //SeedOrganziations();
            //SeedVolunteers();
        }

        private void SeedRegions()
        {
            var regions = SeedDataLoader.GetSeedData<List<Region>>("Regions");

            _seederRepository.AddOrUpdateRegions(regions);
        }

        private void SeedRegionalDistricts()
        {
            var regionalDistricts = SeedDataLoader.GetSeedData<List<RegionalDistrict>>("RegionalDistricts");
            var regions = _seederRepository.GetRegions().ToList();

            regionalDistricts.ToList().ForEach(rd =>
            {
                rd.RegionId = regions.Single(r => r.Name.Equals(rd.Name, StringComparison.OrdinalIgnoreCase)).Id;
                rd.Region = null;
            });

            _seederRepository.AddOrUpdateRegionalDistricts(regionalDistricts.GroupBy(rd => new { rd.Name, rd.Id }).Select(g => g.First()).ToList());
        }

        private void SeedCommunities()
        {
            var communities = SeedDataLoader.GetSeedData<List<Community>>("Communities");

            var regionalDistricts = _seederRepository.GetRegionalDistricts().ToList();

            communities.ToList().ForEach(c =>
                {
                    c.RegionalDistrictId = regionalDistricts.Single(rd => rd.Name.Equals(c.RegionalDistrict.Name, StringComparison.OrdinalIgnoreCase)).Id;
                    c.RegionalDistrict = null;
                }
            );

            _seederRepository.AddOrUpdateCommunities(communities
                .GroupBy(c => new { c.Name, c.RegionalDistrictId })
                .Select(g => g.First())
                .ToList());
        }

        private void SeedIncidentTasks()
        {
            var incidentTasks = SeedDataLoader.GetSeedData<List<IncidentTask>>("IncidentTasks");
            var regions = _seederRepository.GetRegions().ToList() ?? new List<Region>();
            var regionalDistricts = _seederRepository.GetRegionalDistricts().ToList() ?? new List<RegionalDistrict>();
            var incidentTaskCommunities = incidentTasks.Where(it => it.Community != null).Select(s => s.Community).ToList() ?? new List<Community>();
            var communties = _seederRepository.GetCommunities().Where(c => incidentTaskCommunities.Exists(it => it.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)));

            incidentTasks.ToList().ForEach(it =>
                {
                    it.RegionId = regions.SingleOrDefault(r => r.Name.Equals(it.Region?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    it.Region = null;
                    it.RegionalDistrictId = regionalDistricts
                        .SingleOrDefault(rd => rd.Name.Equals(it.RegionalDistrict?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    it.RegionalDistrict = null;
                    it.CommunityId = communties.SingleOrDefault(c => c.Name.Equals(it.Community?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    it.Community = null;
                }
            );

            _seederRepository.AddOrUpdateIncidentTasks(incidentTasks
                .GroupBy(c => c.TaskNumber)
                .Select(g => g.First())
                .ToList());
        }

        private void SeedFamilyRelationshipType()
        {
            var relationshipTypes = SeedDataLoader.GetSeedData<List<FamilyRelationshipType>>("FamilyRelationshipTypes");

            _seederRepository.AddOrUpdateFamilyRelationshipTypes(relationshipTypes.GroupBy(r => r.Code).Select(g => g.First()).ToList());
        }
        private void SeedCountries()
        {
            var countries = SeedDataLoader.GetSeedData<List<Country>>("Countries");

            _seederRepository.AddOrUpdateCountries(countries.GroupBy(c => c.Name).Select(g => g.First()).ToList());
        }
        private void SeedOrganziations()
        {
            var organizations = SeedDataLoader.GetSeedData<List<Organization>>("Organizations");

            var regions = _seederRepository.GetRegions().ToList() ?? new List<Region>();
            var regionalDistricts = _seederRepository.GetRegionalDistricts().ToList() ?? new List<RegionalDistrict>();
            var organizationCommunities = organizations.Where(it => it.Community != null).Select(s => s.Community).ToList() ?? new List<Community>();
            var communties = _seederRepository.GetCommunities().Where(c => organizationCommunities.Exists(it => it.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)));

            organizations.ToList().ForEach(o =>
                {
                    o.RegionId = regions.SingleOrDefault(r => r.Name.Equals(o.Region?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    o.Region = null;
                    o.RegionalDistrictId = regionalDistricts
                        .SingleOrDefault(rd => rd.Name.Equals(o.RegionalDistrict?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    o.RegionalDistrict = null;
                    o.CommunityId = communties.SingleOrDefault(c => c.Name.Equals(o.Community?.Name, StringComparison.OrdinalIgnoreCase))?.Id;
                    o.Community = null;
                }
            );
        }
        private void SeedVolunteers()
        {
            throw new NotImplementedException();
        }
    }
}
