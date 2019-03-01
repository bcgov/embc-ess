
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gov.Jag.Embc.Public.Sqlite.Models;

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
        public DbSet<BceidUser> BceidUser { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RegionalDistrict> RegionalDistricts { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<SupportType> SupportTypes { get; set; }
    }
}
