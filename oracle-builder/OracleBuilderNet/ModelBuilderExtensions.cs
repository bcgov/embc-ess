using Gov.Jag.Embc.Interfaces.Models;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.History;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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
        public static void UpperCaseUnderscoreSingularConvention(this DbModelBuilder modelBuilder)
        {
            // table names should be plural
            // field names do not have a prefix
            // max field length of 30 characters

            modelBuilder.Types().Configure(entityType =>
            {
                if (entityType.ClrType != null)
                { 

                entityType.ToTable(TABLE_PREFIX + ConvertName(entityType.ClrType.Name));
                

                }
            });

            modelBuilder.Properties().Configure(
                property =>
                {
                    property.HasColumnName(ConvertName(property.ClrPropertyInfo.Name));
                });
                
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
            name = name.Replace("gov_", "");
            // first add the underscore
            string result = Regex.Replace(name, "([^_A-Z])([A-Z])", "$1_$2");
            // then convert to uppercase.  
            result = result.ToUpperInvariant();
            return result;
        }
    }
}