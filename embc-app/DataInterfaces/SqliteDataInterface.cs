using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public class SqliteDataInterface : IDataInterface
    {
        SqliteDataInterface()
        {
            // init the database.

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
