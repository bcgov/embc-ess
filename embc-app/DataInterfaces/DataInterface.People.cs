using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private IQueryable<Models.Db.Person> GetAllPeopleAsync(string type)
        {
            IQueryable<Models.Db.Person> result = null;
            if (type == Models.Db.Person.VOLUNTEER)
            {
                result = db.People.Include(x => ((Models.Db.Volunteer)x).Organization)
                    .Where(p => p.PersonType.Equals(type, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                result = db.People.Where(p => p.PersonType.Equals(type, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }

        private async Task<Models.Db.Person> GetSinglePersonByIdAsync(string type, string id)
        {
            Models.Db.Person result = null;
            if (type == Models.Db.Person.VOLUNTEER)
            {
                result = await db.People
                    .Include(x => ((Models.Db.Volunteer)x).Organization)
                    .FirstOrDefaultAsync(p => p.PersonType.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Id == Guid.Parse(id));
            }
            else
            {
                result = await db.People.FirstOrDefaultAsync(p => p.PersonType.Equals(type, StringComparison.OrdinalIgnoreCase) && p.Id == Guid.Parse(id));
            }
            return result;
        }

        public async Task UpdatePersonAsync(Person person)
        {
            db.People.Update(person.ToModel());
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync(string type)
        {
            return await GetAllPeopleAsync(type).Select(p => p.ToViewModel()).ToArrayAsync();
        }

        public async Task<Person> GetPersonByIdAsync(string type, string id)
        {
            var person = await GetSinglePersonByIdAsync(type, id);
            if (person == null) return null;
            return person.ToViewModel();
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            var newPerson = await db.People.AddAsync(person.ToModel());
            await db.SaveChangesAsync();
            return newPerson.Entity.ToViewModel();
        }

        public async Task<bool> DeactivatePersonAsync(string type, string id)
        {
            var person = await GetSinglePersonByIdAsync(type, id);
            if (person == null) return false;
            person.Active = false;
            db.Update(person);
            await db.SaveChangesAsync();
            return true;
        }

        public Volunteer GetVolunteerByBceidUserId(string bceidUserId)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => ((Models.Db.Volunteer)x).BceidAccountNumber == bceidUserId);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerByExternalId(string externalId)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => ((Models.Db.Volunteer)x).Externaluseridentifier == externalId);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerByName(string firstName, string lastName)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerById(string id)
        {
            Volunteer result = null;
            Guid guid = new Guid(id);
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => x.Id == guid);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }
    }
}
