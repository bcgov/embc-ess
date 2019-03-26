using Microsoft.Extensions.Configuration;
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

            var db = GetDatabaseName(Configuration);

            var auth = string.IsNullOrEmpty(Configuration["DB_USER"])
                ? "Trusted_Connection=True"
                : "User Id=" + Configuration["DB_USER"] + ";Password=" + Configuration["DB_PASSWORD"];

            return $"Server={server};Database={db};{auth};MultipleActiveResultSets=true;";
        }

        public static string GetSaConnectionString(IConfiguration Configuration)
        {
            if (string.IsNullOrEmpty(Configuration["DB_ADMIN_PASSWORD"])) return GetConnectionString(Configuration);
            var server = string.IsNullOrEmpty(Configuration["DATABASE_SERVICE_NAME"]) ? "(localdb)\\mssqllocaldb" : Configuration["DATABASE_SERVICE_NAME"];

            var db = GetDatabaseName(Configuration);

            var auth = string.IsNullOrEmpty(Configuration["DB_USER"])
                ? "Trusted_Connection=True"
                : "User Id=sa;Password=" + Configuration["DB_ADMIN_PASSWORD"];

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
                string sql = "EXEC sp_configure 'show advanced options', 1;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "RECONFIGURE WITH OVERRIDE;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "EXEC sp_configure 'max degree of parallelism', 2;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "RECONFIGURE WITH OVERRIDE;";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // create the login if it does not exist.
                sql = "IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = '" + username + "') BEGIN\n CREATE LOGIN " + username + " WITH PASSWORD = '" + password + "';\nEND";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "IF  NOT EXISTS(SELECT name FROM sys.databases WHERE name = N'" + database + "')\nBEGIN\nCREATE DATABASE[" + database + "]; ALTER AUTHORIZATION ON DATABASE::[" + database + "] TO " + username + "\nEND";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                sql = "USE " + database + "; IF NOT EXISTS (SELECT su.name as DatabaseUser FROM sys.sysusers su join sys.syslogins sl on sl.sid = su.sid where sl.name = '" + username + "')\nBEGIN\nCREATE USER " + username + " FOR LOGIN " + username + ";END";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        /// <summary>
        /// Returns the name of the database, as set in the environment.
        /// </summary>
        /// <returns></returns>
        private static string GetDatabaseName(IConfiguration Configuration)
        {
            string result = "";
            if (!string.IsNullOrEmpty(Configuration["DB_DATABASE"]))
            {
                result += Configuration["DB_DATABASE"];
            }
            else // default to a local connection.
            {
                result += "ESS";
            }

            return result;
        }
    }
}
