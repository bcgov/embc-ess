using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class SeederRepository : ISeederRepository
    {
        private readonly EmbcDbContext db;

        public SeederRepository(EmbcDbContext ctx)
        {
            db = ctx;
        }

        public IEnumerable<Country> GetCountries()
        {
            return db.Countries.ToArray();
        }

        public IEnumerable<Region> GetRegions()
        {
            return db.Regions.ToArray();
        }

        public void AddOrUpdateRegions(IEnumerable<Region> regions)
        {
            var existingEntities = db.Regions.Where(r => regions.Any(ex => ex.Name.Equals(r.Name, StringComparison.OrdinalIgnoreCase))).ToList();

            foreach (var entity in existingEntities)
            {
                var region = regions.Single(r => r.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase));
                entity.Name = region.Name;
                entity.Active = region.Active;
                db.AddOrUpdate(entity);
            }
            db.UpdateRange(existingEntities);

            var newEntities = regions.Where(r => !existingEntities.Exists(ex => ex.Name.Equals(r.Name, StringComparison.OrdinalIgnoreCase)));
            db.AddRange(newEntities);

            db.SaveChanges();
        }

        public void AddOrUpdateFamilyRelationshipTypes(IEnumerable<FamilyRelationshipType> familyRelationshipTypes)
        {
            var existingEntities = db.FamilyRelationshipTypes.Where(f => familyRelationshipTypes.Any(ex => ex.Code == f.Code)).ToList();

            foreach (var entity in existingEntities)
            {
                var familyRelationshipType = db.FamilyRelationshipTypes.SingleOrDefault(f => f.Code == entity.Code);
                entity.Code = familyRelationshipType.Code;
                entity.Active = familyRelationshipType.Active;
                entity.Description = familyRelationshipType.Description;
                db.AddOrUpdate(entity);
            }
            db.UpdateRange(existingEntities);

            var newEntities = familyRelationshipTypes.Where(f => !existingEntities.Exists(ex => ex.Code == f.Code));
            db.AddRange(newEntities);

            db.SaveChanges();
        }

        public IEnumerable<Community> GetCommunities()
        {
            return db.Communities.ToArray();
        }

        public void AddOrUpdateCommunities(IEnumerable<Community> communities)
        {
            var existingEntities = db.Communities
                .Where(ex =>
                            communities.Any(
                                c => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) &&
                                (
                                    (!string.IsNullOrEmpty(ex.RegionName) && c.RegionName == ex.RegionName) || string.IsNullOrEmpty(ex.RegionName)
                                )
                            )
                ).ToList();

            foreach (var entity in existingEntities)
            {
                var updatedCommunity = default(Community);
                if (!string.IsNullOrEmpty(entity.RegionName))
                {
                    updatedCommunity = communities.Single(c => c.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                                c.RegionName == entity.RegionName);
                }
                else
                {
                    updatedCommunity = communities.First(c => c.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                               !existingEntities.Exists(ex => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                                                        !string.IsNullOrEmpty(ex.RegionName) && c.RegionName == ex.RegionName));
                }

                entity.Name = updatedCommunity.Name;
                entity.Active = updatedCommunity.Active;
                entity.RegionName = updatedCommunity.RegionName;
            }

            var newEntities = communities
                .Where(c => !existingEntities.Exists(ex => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) && c.RegionName == ex.RegionName));

            db.AddRange(newEntities);
            db.UpdateRange(existingEntities);

            db.SaveChanges();
        }

        public void AddOrUpdateIncidentTasks(IEnumerable<IncidentTask> incidentTasks)
        {
            var existingEntities = db.IncidentTasks.Where(it => incidentTasks.Any(ex => ex.TaskNumber == it.TaskNumber)).ToList();

            foreach (var entity in existingEntities)
            {
                var incidentTask = incidentTasks.Single(it => it.TaskNumber == entity.TaskNumber);
                entity.TaskNumber = incidentTask.TaskNumber;
                entity.Details = incidentTask.Details;
                entity.Active = incidentTask.Active;
                entity.RegionName = incidentTask.RegionName;
                entity.CommunityId = incidentTask.CommunityId;
                entity.StartDate = incidentTask.StartDate;
            }
            db.UpdateRange(existingEntities);

            var newEntities = incidentTasks.Where(it => !existingEntities.Exists(ex => ex.TaskNumber == it.TaskNumber));
            db.AddRange(newEntities);

            db.SaveChanges();
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            return db.Organizations.ToArray();
        }

        public void AddOrUpdateOrganizations(IEnumerable<Organization> organizations)
        {
            var existingEntities = db.Organizations.Where(o => organizations.Any(ex => ex.BCeIDBusinessGuid == o.BCeIDBusinessGuid)).ToList();

            foreach (var entity in existingEntities)
            {
                var organization = existingEntities.Single(o => o.BCeIDBusinessGuid == entity.BCeIDBusinessGuid);

                entity.BCeIDBusinessGuid = organization.BCeIDBusinessGuid;
                entity.Name = organization.Name;
                entity.RegionName = organization.RegionName;
                entity.CommunityId = organization.CommunityId;
            }
            db.UpdateRange(existingEntities);

            var newEntities = organizations.Where(o => !existingEntities.Exists(ex => ex.BCeIDBusinessGuid == o.BCeIDBusinessGuid));
            db.AddRange(newEntities);

            db.SaveChanges();
        }

        public void AddOrUpdateVolunteers(IEnumerable<Volunteer> volunteers)
        {
            var existingEntities = db.Volunteers
                    .Where(v => volunteers.Any(ex => ex.BceidAccountUserName == v.BceidAccountUserName))
                    .ToList();

            foreach (var entity in existingEntities)
            {
                var volunteer = volunteers.Single(x => x.BceidAccountUserName == entity.BceidAccountUserName);

                entity.BceidAccountUserName = volunteer.BceidAccountUserName;
                entity.FirstName = volunteer.FirstName;
                entity.LastName = volunteer.LastName;
                entity.UserId = volunteer.UserId;
                entity.Active = volunteer.Active;
                entity.CanAccessRestrictedFiles = volunteer.CanAccessRestrictedFiles;
                entity.Email = volunteer.Email;
                entity.OrganizationId = volunteer.OrganizationId;
                entity.IsAdministrator = volunteer.IsAdministrator;
                entity.IsPrimaryContact = volunteer.IsPrimaryContact;
            }
            db.UpdateRange(existingEntities);

            var newEntities = volunteers.Where(v => !existingEntities.Exists(ex => ex.BceidAccountUserName == v.BceidAccountUserName));
            db.AddRange(newEntities);

            db.SaveChanges();
        }

        public void AddOrUpdateCountries(IEnumerable<Country> countries)
        {
            var existingEntities = db.Countries
                .Where(ex => countries.Any(c => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase))).ToList();
            foreach (var entity in existingEntities)
            {
                var updateCountry = countries.Single(c => c.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase));
                entity.Name = updateCountry.Name;
                entity.Active = updateCountry.Active;
            }

            var newEntities = countries.Where(c => !existingEntities.Exists(ex => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase)));

            db.AddRange(newEntities);
            db.UpdateRange(existingEntities);

            db.SaveChanges();
        }
    }
}
