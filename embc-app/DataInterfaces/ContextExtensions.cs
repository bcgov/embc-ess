using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ContextExtensions
    {
        public static void AddOrUpdate(this DbContext ctx, object entity)
        {
            var entry = ctx.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    ctx.Add(entity);
                    break;

                case EntityState.Modified:
                    ctx.Update(entity);
                    break;

                case EntityState.Added:
                    ctx.Add(entity);
                    break;

                case EntityState.Unchanged:
                    //item already in db no need to do anything
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

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
            modelBuilder.Entity<T>().Property<string>("CreatedByUserId")
                .HasMaxLength(255)
                .IsRequired()
                .HasDefaultValueSql("'System'");
            modelBuilder.Entity<T>().Property<DateTime>("UpdateDateTime").IsRequired().HasDefaultValueSql("GetUtcDate()");
            modelBuilder.Entity<T>().Property<string>("UpdatedByUserId")
                .HasMaxLength(255)
                .IsRequired()
                .HasDefaultValueSql("'System'");
        }
    }
}
