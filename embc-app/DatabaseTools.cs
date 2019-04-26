using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace Gov.Jag.Embc.Public
{
    public static class DatabaseTools
    {
        /// <summary>
        /// Logic required to generate a connection string.  If no environment variables exists, defaults to a local sql instance.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString(IConfiguration Configuration)
        {
            var server = string.IsNullOrEmpty(Configuration["DATABASE_SERVICE_NAME"]) ? "(localdb)\\mssqllocaldb" : Configuration["DATABASE_SERVICE_NAME"];

            var db = string.IsNullOrEmpty(Configuration["DB_DATABASE"]) ? "ESS" : Configuration["DB_DATABASE"];

            var auth = string.IsNullOrEmpty(Configuration["DB_USER"])
                ? "Trusted_Connection=True"
                : "User Id=" + Configuration["DB_USER"] + ";Password=" + Configuration["DB_PASSWORD"];

            return $"Server={server};Database={db};{auth};MultipleActiveResultSets=true;";
        }

        public static string GetSaConnectionString(IConfiguration Configuration, string toDatabase = null)
        {
            if (string.IsNullOrEmpty(Configuration["DB_ADMIN_PASSWORD"])) return GetConnectionString(Configuration);
            var server = string.IsNullOrEmpty(Configuration["DATABASE_SERVICE_NAME"]) ? "(localdb)\\mssqllocaldb" : Configuration["DATABASE_SERVICE_NAME"];

            var db = toDatabase ?? (string.IsNullOrEmpty(Configuration["DB_DATABASE"]) ? "ESS" : Configuration["DB_DATABASE"]);

            var auth = string.IsNullOrEmpty(Configuration["DB_USER"])
                ? "Trusted_Connection=True"
                : "User Id=SA;Password=" + Configuration["DB_ADMIN_PASSWORD"];

            return $"Server={server};Database={db};{auth};MultipleActiveResultSets=true;";
        }

        /// <summary>
        /// Create database if it does not exist - used in OpenShift or other environments that do not automatically create the database.
        /// </summary>
        public static void CreateDatabaseIfNotExists(string adminConnectionString, string database, string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(adminConnectionString))
            {
                conn.Open();
                // fix for OpenShift bug where the pod reports the number of sockets / logical processors in the host computer rather than the amount available.
                string sql = "USE master; EXEC sp_configure 'show advanced options', 1;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE master; RECONFIGURE WITH OVERRIDE;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE master; EXEC sp_configure 'max degree of parallelism', 2;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE master; RECONFIGURE WITH OVERRIDE;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create the login if it does not exist.
                sql = "USE master; IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = '" + username + "') BEGIN\n CREATE LOGIN " + username + " WITH PASSWORD = '" + password + "';\nEND";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE master; IF  NOT EXISTS(SELECT name FROM sys.databases WHERE name = N'" + database + "')\nBEGIN\nCREATE DATABASE[" + database + "]; ALTER AUTHORIZATION ON DATABASE::[" + database + "] TO " + username + "\nEND";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE " + database + "; IF NOT EXISTS (SELECT su.name as DatabaseUser FROM sys.sysusers su join sys.syslogins sl on sl.sid = su.sid where sl.name = '" + username + "')\nBEGIN\nCREATE USER " + username + " FOR LOGIN " + username + ";END";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void SyncInitialMigration(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var sql =
                    @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'__EFMigrationsHistory')
                        BEGIN
                                IF NOT EXISTS(SELECT ef.MigrationId FROM __EFMigrationsHistory ef WHERE ef.MigrationId = N'20190424150858_InitialDB')
                                BEGIN
                                    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                                    VALUES(N'20190424150858_InitialDB', N'2.2.0-rtm-35687');
                                END
                        END";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
