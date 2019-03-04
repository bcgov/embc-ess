using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public class SqliteDataInterface : IDataInterface
    {            
        public SqliteContext Db;

        public SqliteDataInterface(string connectionString)
        {

            DbContextOptionsBuilder<SqliteContext> builder = new DbContextOptionsBuilder<SqliteContext>();
            
            builder.UseSqlite(connectionString);

            // init the database.
            Db = new SqliteContext(builder.Options);
            
            Db.Database.OpenConnection();

            
            
        }

        public Person CreatePerson(Person person)
        {
            return person;
        }

        public Registration CreateRegistration(Registration registration)
        {
            var model = registration.ToModel();
            Db.Registrations.Add(model);
            Db.SaveChanges();
            return model.ToViewModel();
        }

        public Organisation GetOrganisationByBceidGuid(string bceidGuid)
        {
            Organisation result = new Organisation();
            return result;
        }

        public Person GetPersonByBceidGuid(string bceidGuid)
        {
            Person result = new Person();
            return result;
        }        

        public List<Region> GetRegions()
        {
            List<Region> regions = new List<Region>();
            var regionList = Db.Regions.ToList();
            foreach (var region in regionList)
            {
                regions.Add(region.ToViewModel());
            }
            return regions;   
        }

        public List<Registration> GetRegistrations()
        {
            List<Registration> regions = new List<Registration>();
            var registrationList = Db.Registrations.ToList();
            foreach (var registration in registrationList)
            {
                regions.Add(registration.ToViewModel());
            }
            return regions;
        }

        public List<RegionalDistrict> GetRegionalDistricts()
        {
            List<RegionalDistrict> regions = new List<RegionalDistrict>();
            var regionalDistrictList = Db.RegionalDistricts.ToList();
            foreach (var regionalDistrict in regionalDistrictList)
            {
                regions.Add(regionalDistrict.ToViewModel());
            }
            return regions;
        }

        public List<Community> GetCommunities()
        {
            List<Community> regions = new List<Community>();
            var communityList = Db.Communities.ToList();
            foreach (var community in communityList)
            {
                regions.Add(community.ToViewModel());
            }
            return regions;
        }

    }
}
