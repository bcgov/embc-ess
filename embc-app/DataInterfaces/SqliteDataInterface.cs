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

        public void CreatePerson(Person person)
        {

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

    }
}
