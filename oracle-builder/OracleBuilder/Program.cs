using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace OracleBuilder
{
    /// <summary>
    /// Utility Program to backfill data in the event of a problem with BCeID
    /// This tool can also be used to generate an export of active users
    /// </summary>
    class Program
    {
       
        static void Main(string[] args)
        {
            string sqlConnectionString = GetConnectionString();
            
            DbContextOptionsBuilder<DbAppContext> builder = new DbContextOptionsBuilder<DbAppContext>();
            //builder.UseSqlServer(sqlConnectionString);
            builder.UseOracle();
            

            using (var db = new DbAppContext(builder.Options))
            {
                db.Database.OpenConnection();
                db.Database.EnsureCreated();
            }
            
        }

        public static string GetConnectionString()
        {
            string result = "Server=127.0.0.1\\SQLEXPRESS;Database=ess;User Id=ess;Password=test;";

            return result;
        }
    }
}
