using Gov.Jag.Embc.Public.Models.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class FamilyRelationshipTypeExtensions
    {
        public static void PatchValues(this FamilyRelationshipType self, FamilyRelationshipType other)
        {
            self.Code = other.Code;
            self.Description = other.Description;
            self.Active = other.Active;
        }

        public static void AddFamilyRelationshipType(this EmbcDbContext context, FamilyRelationshipType entity)
        {
            // create a new FamilyRelationshipType.
            context.FamilyRelationshipTypes.Add(entity);
            context.SaveChanges();
        }

        public static void UpdateFamilyRelationshipType(this EmbcDbContext context, FamilyRelationshipType value)
        {
            FamilyRelationshipType entityFound = context.GetFamilyRelationshipTypeByCode(value.Code);
            if (entityFound != null)
            {
                entityFound.PatchValues(value);
                context.FamilyRelationshipTypes.Update(entityFound);
                context.SaveChanges();
            }
        }

        public static List<FamilyRelationshipType> GetFamilyRelationshipTypes(this EmbcDbContext context)
        {
            List<FamilyRelationshipType> FamilyRelationshipTypes =
                context.FamilyRelationshipTypes.ToList<FamilyRelationshipType>();
            return FamilyRelationshipTypes;
        }

        /// <summary>
        /// Returns a specific FamilyRelationshipType
        /// </summary>
        /// <param name="code">The name of the FamilyRelationshipType</param>
        /// <returns>The FamilyRelationshipType, or null if it does not exist.</returns>
        public static FamilyRelationshipType GetFamilyRelationshipTypeByCode(this EmbcDbContext context, string code)
        {
            FamilyRelationshipType entity = context.FamilyRelationshipTypes.FirstOrDefault(x => x.Code == code);
            return entity;
        }

        /// <summary>
        /// Create FamilyRelationshipTypes from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonFilePath"></param>
        public static void AddInitialFamilyRelationshipTypesFromFile(this EmbcDbContext context, string jsonFilePath)
        {
            if (!string.IsNullOrEmpty(jsonFilePath) && File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                context.AddInitialFamilyRelationshipTypes(json);
            }
        }

        private static void AddInitialFamilyRelationshipTypes(this EmbcDbContext context, string json)
        {
            List<FamilyRelationshipType> values = JsonConvert.DeserializeObject<List<FamilyRelationshipType>>(json);

            if (values != null)
            {
                context.AddInitialFamilyRelationshipTypes(values);
            }
        }

        private static void AddInitialFamilyRelationshipTypes(this EmbcDbContext context, List<FamilyRelationshipType> values)
        {
            values.ForEach(context.AddInitialFamilyRelationshipType);
        }

        /// <summary>
        /// Adds a FamilyRelationshipType to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialFamilyRelationshipType(this EmbcDbContext context, FamilyRelationshipType value)
        {
            FamilyRelationshipType entity = context.GetFamilyRelationshipTypeByCode(value.Code);
            if (entity != null)
            {
                return;
            }

            entity = new FamilyRelationshipType();
            entity.PatchValues(value);
            context.AddFamilyRelationshipType(entity);
        }

        /// <summary>
        /// Update FamilyRelationshipType
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        public static void UpdateSeedFamilyRelationshipTypeInfo(this EmbcDbContext context, FamilyRelationshipType value)
        {
            FamilyRelationshipType foundEntity = context.GetFamilyRelationshipTypeByCode(value.Code);
            if (foundEntity == null)
            {
                context.AddFamilyRelationshipType(value);
            }
            else
            {
                foundEntity.PatchValues(value);
                context.UpdateFamilyRelationshipType(foundEntity);
            }
        }
    }
}
