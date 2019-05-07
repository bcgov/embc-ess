using Gov.Jag.Embc.Public.Authentication;
using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    /// <summary>
    /// Database Context Factory Interface
    /// </summary>
    public interface IEmbcDbContextFactory
    {
        /// <summary>
        /// Create new database context
        /// </summary>
        /// <returns></returns>
        EmbcDbContext Create();
    }

    /// <summary>
    /// Database Context Factory
    /// </summary>
    public class EmbcDbContextFactory : IEmbcDbContextFactory
    {
        private readonly DbContextOptions<EmbcDbContext> options;
        private readonly IHttpContextAccessor ctx;

        /// <summary>
        /// Database Context Factory Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="options"></param>
        public EmbcDbContextFactory(IHttpContextAccessor httpContextAccessor, DbContextOptions<EmbcDbContext> options)
        {
            this.options = options;
            ctx = httpContextAccessor;
        }

        /// <summary>
        /// Create new database context
        /// </summary>
        /// <returns></returns>
        public EmbcDbContext Create()
        {
            return new EmbcDbContext(ctx, options);
        }
    }

    public class EmbcDbContext : DbContext
    {
        private readonly IHttpContextAccessor ctx;

        public EmbcDbContext(DbContextOptions<EmbcDbContext> options) : base(options)
        {
            // override the default timeout as some operations are time intensive
            Database?.SetCommandTimeout(180);
        }

        public EmbcDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<EmbcDbContext> options) : base(options)
        {
            ctx = httpContextAccessor;

            // override the default timeout as some operations are time intensive
            Database?.SetCommandTimeout(180);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .AddEnvironmentVariables()
                   .Build();
                string connectionString = DatabaseTools.GetConnectionString(configuration);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Evacuee> Evacuees { get; set; }
        public DbSet<FamilyRelationshipType> FamilyRelationshipTypes { get; set; }
        public DbSet<EvacueeRegistration> EvacueeRegistrations { get; set; }
        public DbSet<IncidentTask> IncidentTasks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<EvacueeRegistrationAddress> EvacueeRegistrationAddresses { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this line is required so ef migrations will work.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evacuee>()
                .HasKey(e => new { e.EvacueeRegistrationId, e.EvacueeSequenceNumber });

            modelBuilder.Entity<EvacueeRegistrationAddress>()
                .HasKey(ira => new { ira.EvacueeRegistrationId, ira.AddressSequenceNumber });

            // Address hierarchy
            modelBuilder.Entity<BcAddress>().HasBaseType<Address>();
            modelBuilder.Entity<OtherAddress>().HasBaseType<Address>();
            modelBuilder.Entity<Address>()
                .ToTable("Addresses")
                .HasDiscriminator(addr => addr.AddressSubtype)
                .HasValue<BcAddress>(Address.BC_ADDRESS)
                .HasValue<OtherAddress>(Address.OTHER_ADDRESS);

            // People hierarchy
            modelBuilder.Entity<HeadOfHousehold>().HasBaseType<Person>();
            modelBuilder.Entity<FamilyMember>().HasBaseType<Person>();
            modelBuilder.Entity<Person>()
                .ToTable("People")
                .HasDiscriminator(pers => pers.PersonType)
                .HasValue<HeadOfHousehold>(Person.HOH)
                .HasValue<FamilyMember>(Person.FAMILY_MEMBER);

            modelBuilder.HasSequence<long>("ESSFileNumbers")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<Registration>()
                .Property(r => r.EssFileNumber)
                .HasDefaultValueSql("NEXT VALUE FOR ESSFileNumbers");

            modelBuilder.ShadowProperties();

        }

        public override int SaveChanges()
        {
            SetShadowProperties(ctx?.HttpContext?.User);

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetShadowProperties(ctx?.HttpContext?.User);

            return await base.SaveChangesAsync(cancellationToken);
        }

        public void SetShadowProperties(ClaimsPrincipal principal)
        {
            var timestamp = DateTime.UtcNow;
            var userId = principal?.FindFirstValue(EssClaimTypes.USER_ID) ?? "System";
            foreach (var entry in ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified)))
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
