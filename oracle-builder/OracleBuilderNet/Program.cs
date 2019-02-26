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
using Gov.Jag.Embc.Interfaces.Models;

namespace OracleBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbAppContext>());

            using (var ctx = new DbAppContext())
            {
                var region = new Region()
                {
                    Name = "Vancouver Island"
                };
                ctx.Regions.Add(region);

                ctx.SaveChanges();
                var community = new Community()
                {
                    Name = "Victoria"
                };
                ctx.Communities.Add(community);

                ctx.SaveChanges();

                

                var person = new People()
                {
                    Firstname = "Person",
                    Lastname = "Test"
                    
                };
                ctx.SaveChanges();
                var organization = new Organisation()
                {
                    Name = "TestOrg",
                    PrimaryContact = person
                };

                ctx.People.Add(person);



                ctx.SaveChanges();
            }

            

            

        }
    }
}
