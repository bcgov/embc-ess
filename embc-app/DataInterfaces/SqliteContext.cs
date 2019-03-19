using Gov.Jag.Embc.Public.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {
            // override the default timeout as some operations are time intensive
            Database?.SetCommandTimeout(180);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FamilyRelationshipType> FamilyRelationshipTypes { get; set; }
        public DbSet<IncidentTask> IncidentTasks { get; set; }
        public DbSet<RegionalDistrict> RegionalDistricts { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        // public DbSet<HeadOfHousehold> HeadOfHouseholds { get; set; }
        // public DbSet<FamilyMember> FamilyMembers { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyRelationshipType>().HasKey(k => k.Code);

            // Address hierarchy
            modelBuilder.Entity<BcAddress>().HasBaseType<Address>();
            modelBuilder.Entity<OtherAddress>().HasBaseType<Address>();
            modelBuilder.Entity<Address>()
                .ToTable("Addresses")
                .HasDiscriminator(addr => addr.AddressSubtype)
                .HasValue<BcAddress>("BCAD")
                .HasValue<OtherAddress>("OTAD");

            // People hierarchy
            modelBuilder.Entity<Volunteer>().HasBaseType<Person>();
            modelBuilder.Entity<HeadOfHousehold>().HasBaseType<Person>();
            modelBuilder.Entity<FamilyMember>().HasBaseType<Person>();
            modelBuilder.Entity<Person>()
                .ToTable("People")
                .HasDiscriminator(pers => pers.PersonType)
                .HasValue<Volunteer>("VOLN")
                .HasValue<HeadOfHousehold>("HOH")
                .HasValue<FamilyMember>("FMBR");

            // TODO: Specify any additional inheritance hierarchies here!
        }
    }
}
