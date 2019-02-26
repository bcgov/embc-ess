using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace OracleBuilder
{
    /// <summary>
    /// Utility extension added to entity framework
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// The table prefix for this application
        /// </summary>
        public const string TABLE_PREFIX = "ESS_"; 

        /// <summary>
        /// Implements the following naming convention:
        /// Camel Case converted to upper case, with underscores between words.
        /// Tables have an application specific prefix.
        /// Primary keys have the table name as a prefix
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void UpperCaseUnderscoreSingularConvention(this ModelBuilder modelBuilder)
        {            
            // table names should be plural
            // field names do not have a prefix
            // max field length of 30 characters

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip shadow types
                if (entityType.ClrType == null)
                    continue;

                entityType.Relational().TableName = TABLE_PREFIX + ConvertName(entityType.ClrType.Name);

                string primaryKeyName = entityType.ClrType.Name.Replace("MicrosoftDynamicsCRM", "") + "id";

                // Now convert the column names.
                foreach (var entityProperty in entityType.GetProperties())
                {
                    // Special case for the primary key.
                    // Primary key has a prefix of the table name, excluding the application prefix.
                    if (entityProperty.Name != null && entityProperty.Name.ToLowerInvariant().Equals(primaryKeyName))
                    {
                        entityProperty.Relational().ColumnName = ConvertName(entityType.ClrType.Name) + "_ID";
                    }
                    else
                    {
                        entityProperty.Relational().ColumnName = ConvertName(entityProperty.Name);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Utility function to perform the text conversion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static String ConvertName(String name)
        {

            // first remove the CRM prefix.  Two calls are necessary to get the custom and primitive entities
            name = name.Replace("MicrosoftDynamicsCRMgov", "");
            name = name.Replace("MicrosoftDynamicsCRM", "");
            // first add the underscore
            string result = Regex.Replace(name, "([^_A-Z])([A-Z])", "$1_$2");
            // then convert to uppercase.  
            result = result.ToUpperInvariant();
            return result;
        }
    }
}