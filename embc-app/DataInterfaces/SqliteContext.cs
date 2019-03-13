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

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }
        */

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<HeadOfHousehold> HeadOfHouseholds { get; set; }
        public DbSet<IncidentTask> IncidentTasks { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<RegionalDistrict> RegionalDistricts { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
