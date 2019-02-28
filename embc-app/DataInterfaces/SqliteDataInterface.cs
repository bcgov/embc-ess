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
        private SqliteContext db;

        public SqliteDataInterface(string connectionString)
        {

            DbContextOptionsBuilder<SqliteContext> builder = new DbContextOptionsBuilder<SqliteContext>();
            
            builder.UseSqlite(connectionString);

            // init the database.
            db = new SqliteContext(builder.Options);
            
            db.Database.OpenConnection();
            db.Database.EnsureCreated();
            
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

    }
}
