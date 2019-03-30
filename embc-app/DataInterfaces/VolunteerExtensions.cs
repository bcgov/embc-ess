using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class VolunteerExtensions
    {
        public static void AddVolunteer(this EmbcDbContext context, Volunteer item)
        {
            context.People.Add(item);
            context.SaveChanges();
        }

        public static Volunteer GetVolunteerByBceidAccountNumber(this EmbcDbContext context, string bceidAccountNumber)
        {
            var item = context.People.Where(x => x is Volunteer).Cast<Volunteer>().FirstOrDefault(x => x.BceidAccountNumber == bceidAccountNumber);
            return item;
        }

        public static void AddInitialVolunteersFromFile(this EmbcDbContext context, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                context.AddVolunteers(json);
            }
        }

        private static void AddVolunteers(this EmbcDbContext context, string json)
        {
            List<Volunteer> items = JsonConvert.DeserializeObject<List<Volunteer>>(json);

            if (items != null)
            {
                context.AddInitialVolunteers(items);
            }
        }

        private static void AddInitialVolunteers(this EmbcDbContext context, List<Volunteer> items)
        {
            items.ForEach(context.AddInitialVolunteer);
        }

        private static void AddInitialVolunteer(this EmbcDbContext context, Volunteer item)
        {
            if (context.GetVolunteerByBceidAccountNumber(item.BceidAccountNumber) != null) return;

            Organization org = null;

            if (item.Organization != null)
            {
                org = context.GetOrganizationByBceidAccountNumber(item.Organization.BceidAccountNumber);
            }

            item = new Volunteer
            {
                BceidAccountNumber = item.BceidAccountNumber,
                Active = true,
                OrganizationId = org?.Id,
                Externaluseridentifier = item.Externaluseridentifier,
                Name = item.Name,
                IsAdministrator = item.IsAdministrator,
                CanAccessRestrictedFiles = item.CanAccessRestrictedFiles,
                FirstName = item.FirstName,
                LastName = item.LastName,
                IsPrimaryContact = item.IsPrimaryContact,
                Email = item.Email
            };

            context.AddVolunteer(item);
        }
    }
}
