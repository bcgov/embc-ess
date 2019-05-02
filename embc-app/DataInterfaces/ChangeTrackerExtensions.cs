using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Security.Claims;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(this ChangeTracker changeTracker, ClaimsPrincipal principal)
        {
            var timestamp = DateTime.UtcNow;
            var userId = principal?.FindFirstValue(EssClaimTypes.USER_ID) ?? "System";
            foreach (var entry in changeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified)))
            {
                if (entry.Entity is IAuditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedDateTime").CurrentValue = timestamp;
                        entry.Property("CreatedByUserId").CurrentValue = userId;
                    }

                    entry.Property("UpdateDateTime").CurrentValue = timestamp;
                    entry.Property("UpdatedByUserId").CurrentValue = userId;
                }
            }
        }
    }
}
