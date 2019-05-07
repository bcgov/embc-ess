using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ModelBuilderExtensions
    {
        public static void ShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IAuditableEntity).IsAssignableFrom(clrType))
                {
                    var method = SetAuditingShadowPropertiesMethodInfo.MakeGenericMethod(clrType);
                    method.Invoke(modelBuilder, new object[] { modelBuilder });
                }
            }
        }

        private static readonly MethodInfo SetAuditingShadowPropertiesMethodInfo = typeof(ModelBuilderExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == "SetAuditingShadowProperties");

        public static void SetAuditingShadowProperties<T>(ModelBuilder modelBuilder) where T : class, IAuditableEntity
        {
            modelBuilder.Entity<T>().Property<DateTime>("CreatedDateTime").IsRequired().HasDefaultValueSql("GetUtcDate()");
            modelBuilder.Entity<T>().Property<string>("CreatedByUserId").IsRequired().HasDefaultValueSql("'System'");
            modelBuilder.Entity<T>().Property<DateTime>("UpdateDateTime").IsRequired().HasDefaultValueSql("GetUtcDate()");
            modelBuilder.Entity<T>().Property<string>("UpdatedByUserId").IsRequired().HasDefaultValueSql("'System'");
        }
    }
}
