using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class EmbcSeeder
    {
        readonly ILoggerFactory _loggerFactory;
        private readonly ISeederRepository _seederRepository;

        public EmbcSeeder(ILoggerFactory loggerFactory, ISeederRepository seederRepository)
        {
            _loggerFactory = loggerFactory;
            _seederRepository = seederRepository;
        }

        public void SeedData()
        {
            SeedCommunities();
        }

        private void SeedCommunities()
        {
            var communities = SeedDataLoader.GetSeedData<List<Community>>("Communities");

            var regionalDistricts = _seederRepository.GetRegionalDistricts().ToList();

            communities.ToList().ForEach(c =>
                {
                    c.RegionalDistrictId = regionalDistricts
                    .SingleOrDefault(rd => rd.Name.Equals(c.RegionalDistrict.Name, StringComparison.OrdinalIgnoreCase)).Id;
                    c.RegionalDistrict = null;
                }
            );

            _seederRepository.AddCommunities(communities
                .GroupBy(c => new { c.Name, c.RegionalDistrictId })
                .Select(g => g.First())
                .ToList());
        }
    }
}
