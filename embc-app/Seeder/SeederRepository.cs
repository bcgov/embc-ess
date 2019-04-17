using System;
using System.Collections.Generic;
using System.Linq;
using Gov.Jag.Embc.Public.Data.Extensions;
using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Models.Db;

namespace Gov.Jag.Embc.Public.Seeder
{
    public class SeederRepository : ISeederRepository
    {
        readonly EmbcDbContext db;

        public SeederRepository(EmbcDbContext ctx)
        {
            db = ctx;
        }

        public IEnumerable<Country> GetCountries()
        {
            return db.Countries.ToArray();
        }

        #region Regions

        public IEnumerable<Region> GetRegions()
        {
            return db.Regions.ToArray();
        }

        public void AddOrUpdateRegions(List<Region> regions)
        {
            foreach (var region in regions)
            {
                var entity = db.Regions.SingleOrDefault(r => r.Name.Equals(region.Name, StringComparison.OrdinalIgnoreCase)) ?? new Region();
                entity.Name = region.Name;
                entity.Active = region.Active;
                db.AddOrUpdate(entity);
            }
            
            db.SaveChanges();
        }

        #endregion

        public IEnumerable<RegionalDistrict> GetRegionalDistricts()
        {
            return db.RegionalDistricts.ToArray();
        }

        public void AddOrUpdateRegionalDistricts(List<RegionalDistrict> regionalDistricts)
        {
            foreach (var regionalDistrict in regionalDistricts)
            {
                var entity = db.RegionalDistricts.SingleOrDefault(rd => rd.Name.Equals(regionalDistrict.Name, StringComparison.OrdinalIgnoreCase)) ?? new RegionalDistrict();
                entity.Name = regionalDistrict.Name;
                entity.Active = regionalDistrict.Active;
                entity.RegionId = regionalDistrict.RegionId;
                db.AddOrUpdate(entity);
            }
            db.SaveChanges();
        }

        public void AddOrUpdateFamilyRelationshipTypes(List<FamilyRelationshipType> familyRelationshipTypes)
        {
            foreach (var familyRelationshipType in familyRelationshipTypes)
            {
                var entity = db.FamilyRelationshipTypes.SingleOrDefault(f => f.Code == familyRelationshipType.Code) ?? new FamilyRelationshipType();
                entity.Code = familyRelationshipType.Code;
                entity.Active = familyRelationshipType.Active;
                entity.Description = familyRelationshipType.Description;
                db.AddOrUpdate(entity);
            }
            db.SaveChanges();
        }

        public IEnumerable<Community> GetCommunities()
        {
            return db.Communities.ToArray();
        }

        public void AddOrUpdateCommunities(List<Community> communities)
        {
            var existingEntities = db.Communities
                .Where(ex =>
                            communities.Exists(c => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) && c.RegionalDistrictId == ex.RegionalDistrictId)).ToList();
            foreach (var entity in existingEntities)
            {
                var updatedCommunity = communities.Single(c => c.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase) && c.RegionalDistrictId == entity.RegionalDistrictId);
                entity.Name = updatedCommunity.Name;
                entity.Active = updatedCommunity.Active;
                entity.RegionalDistrictId = updatedCommunity.RegionalDistrictId;
            }

            var newEntities = communities
                .Where(c => !existingEntities.Exists(ex => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase) && c.RegionalDistrictId == ex.RegionalDistrictId)).ToList();

            db.AddRange(newEntities);
            db.UpdateRange(existingEntities);

            db.SaveChanges();
        }

        public void AddOrUpdateIncidentTasks(List<IncidentTask> incidentTasks)
        {
            foreach (var incidentTask in incidentTasks)
            {
                var entity = db.IncidentTasks.SingleOrDefault(it => it.TaskNumber == incidentTask.TaskNumber) ?? new IncidentTask();
                entity.TaskNumber = incidentTask.TaskNumber;
                entity.Details = incidentTask.Details;
                entity.Active = incidentTask.Active;
                entity.RegionId = incidentTask.RegionId;
                entity.RegionalDistrictId = incidentTask.RegionalDistrictId;
                entity.CommunityId = incidentTask.CommunityId;
                db.AddOrUpdate(entity);
            }
            db.SaveChanges();
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            return db.Organizations.ToArray();
        }

        public void AddOrUpdateOrganizations(List<Organization> organizations)
        {
            foreach (var organization in organizations)
            {
                var entity = db.Organizations.SingleOrDefault(o => o.BCeIDBusinessGuid == organization.BCeIDBusinessGuid) ?? new Organization();

                entity.BCeIDBusinessGuid = organization.BCeIDBusinessGuid;
                entity.Name = organization.Name;
                entity.RegionId = organization.RegionId;
                entity.RegionalDistrictId = organization.RegionalDistrictId;
                entity.CommunityId = organization.CommunityId;

                db.AddOrUpdate(entity);
            }

            db.SaveChanges();
        }

        public void AddOrUpdateVolunteers(List<Volunteer> volunteers)
        {
            foreach (var volunteer in volunteers)
            {
                var entity = db.People.Where(x => x is Volunteer)
                    .Cast<Volunteer>()
                    .FirstOrDefault(x => x.BceidAccountNumber == volunteer.BceidAccountNumber) ?? new Volunteer();

                entity.BceidAccountNumber = volunteer.BceidAccountNumber;
                entity.FirstName = volunteer.FirstName;
                entity.LastName = volunteer.LastName;
                entity.Externaluseridentifier = volunteer.Externaluseridentifier;
                entity.Active = volunteer.Active;
                entity.CanAccessRestrictedFiles = volunteer.CanAccessRestrictedFiles;
                entity.Email = volunteer.Email;
                entity.OrganizationId = volunteer.OrganizationId;
                entity.IsAdministrator = volunteer.IsAdministrator;
                entity.IsPrimaryContact = volunteer.IsPrimaryContact;

                db.AddOrUpdate(entity);
            }

            db.SaveChanges();
        }

        public void AddOrUpdateCountries(List<Country> countries)
        {
            var existingEntities = db.Countries
                .Where(ex =>
                            countries.Exists(c => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase))).ToList();
            foreach (var entity in existingEntities)
            {
                var updateCountry = countries.Single(c => c.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase));
                entity.Name = updateCountry.Name;
                entity.Active = updateCountry.Active;
            }

            var newEntities = countries.Where(c => !existingEntities.Exists(ex => ex.Name.Equals(c.Name, StringComparison.OrdinalIgnoreCase))).ToList();

            db.AddRange(newEntities);
            db.UpdateRange(existingEntities);

            db.SaveChanges();
        }

 
    }
}
