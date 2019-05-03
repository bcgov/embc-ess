using Gov.Jag.Embc.Public.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

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
        private readonly DbContextOptions<EmbcDbContext> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Database Context Factory Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="options"></param>
        public EmbcDbContextFactory(IHttpContextAccessor httpContextAccessor, DbContextOptions<EmbcDbContext> options)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Create new database context
        /// </summary>
        /// <returns></returns>
        public EmbcDbContext Create()
        {
            return new EmbcDbContext(_httpContextAccessor, _options);
        }
    }

    public class EmbcDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmbcDbContext(DbContextOptions<EmbcDbContext> options) : base(options)
        {
            // override the default timeout as some operations are time intensive
            Database?.SetCommandTimeout(180);
        }

        public EmbcDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<EmbcDbContext> options) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

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
        public DbSet<IncidentRegistration> IncidentRegistrations { get; set; }
        public DbSet<IncidentTask> IncidentTasks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<IncidentRegistrationAddress> RegistrationAddresses { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this line is required so ef migrations will work.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evacuee>()
                .HasKey(e => new { e.IncidentRegistrationId, e.EvacueeSequenceNumber });

            modelBuilder.Entity<IncidentRegistrationAddress>()
                .HasKey(ira => new { ira.IncidentRegistrationId, ira.AddressSequenceNumber });

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
        }
    }
}
