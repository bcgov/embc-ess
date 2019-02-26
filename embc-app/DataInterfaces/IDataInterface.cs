using Gov.Jag.Embc.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public interface IDataInterface
    {
        Organisation GetOrganisationByBceidGuid(string bceidGuid);

        Person GetPersonByBceidGuid(string bceidGuid);

        void CreatePerson(Person person);

    }
}
