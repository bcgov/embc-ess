using Microsoft.Extensions.Configuration;

namespace Gov.Jag.Embc.Public
{
    public static class ConfigurationEx
    {
        private static string DBConnectionRetry = "Connection Timeout=60;ConnectRetryCount=10;ConnectRetryInterval=5";

        public static int ServerTimeoutInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("SESSION_TIMEOUT_MINUTES", 30);
        }

        public static int UserTimeoutWarningDurationInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("USER_TIMEOUT_WARNING_DURATION_MINUTES", 4);
        }

        public static int UserTimeoutWarningInMinutes(this IConfiguration conf)
        {
            return conf.ServerTimeoutInMinutes() - conf.UserTimeoutWarningDurationInMinutes() - 1;
        }

        public static int DefaultTimeoutWarningInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("DEFAULT_TIMEOUT_WARNING_MINUTES", 5);
        }

        public static int DefaultTimeoutWarningDurationInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("DEFAULT_TIMEOUT_WARNING_DURATION_MINUTES", 2);
        }

        public static bool DbFullRefresh(this IConfiguration conf)
        {
            return conf.GetValue("DB_FULL_REFRESH", "false").ToLowerInvariant() == "true";
        }

        public static bool CspEnabled(this IConfiguration conf)
        {
            return conf.GetValue("CSP_ENABLED", "true").ToLowerInvariant() == "true";
        }

        public static string GetBaseUri(this IConfiguration conf)
        {
            return conf["BASE_URI"];
        }

        public static string GetBasePath(this IConfiguration conf)
        {
            return conf["BASE_PATH"];
        }

        public static string GetEnvironmentTitle(this IConfiguration conf)
        {
            return conf["APP_ENVIRONMENT_TITLE"];
        }

        public static string GetBuildCommitId(this IConfiguration conf)
        {
            return conf["OPENSHIFT_BUILD_COMMIT"];
        }

        public static string GetBuildSource(this IConfiguration conf)
        {
            return conf["OPENSHIFT_BUILD_SOURCE"];
        }

        public static string GetBuildVersion(this IConfiguration conf)
        {
            return conf["OPENSHIFT_BUILD_REFERENCE"];
        }

        public static string GetSiteMinderLogoutUrl(this IConfiguration conf)
        {
            return conf.GetValue("SITEMINDER_LOGOUT_URL", "/");
        }

        public static string GetDbServerName(this IConfiguration conf)
        {
            return conf.GetValue("DATABASE_SERVICE_NAME", "(localdb)\\mssqllocaldb");
        }

        public static string GetDbName(this IConfiguration conf)
        {
            return conf.GetValue("DB_DATABASE", "ESS");
        }

        public static string GetDbUser(this IConfiguration conf)
        {
            return conf["DB_USER"];
        }

        public static string GetDbUserPassword(this IConfiguration conf)
        {
            return conf["DB_PASSWORD"];
        }

        public static string GetDbAdminUserPassword(this IConfiguration conf)
        {
            return conf["DB_ADMIN_PASSWORD"];
        }

        public static bool HasDbAdminPassword(this IConfiguration conf)
        {
            return !string.IsNullOrEmpty(conf.GetDbAdminUserPassword());
        }

        public static string GetDbConnectionString(this IConfiguration conf)
        {
            var user = conf.GetDbUser();
            var auth = string.IsNullOrEmpty(user)
                ? "Trusted_Connection=True"
                : "User Id=" + user + ";Password=" + conf.GetDbUserPassword();

            return $"Server={conf.GetDbServerName()};Database={conf.GetDbName()};{auth};MultipleActiveResultSets=true;{DBConnectionRetry}";
        }

        public static string GetAdminDbConnectionString(this IConfiguration conf, string toDatabase = null)
        {
            if (string.IsNullOrEmpty(conf.GetDbAdminUserPassword())) return conf.GetDbConnectionString();
            var server = conf.GetDbServerName();

            var db = toDatabase ?? conf.GetDbName();

            var auth = string.IsNullOrEmpty(conf.GetDbUser())
                ? "Trusted_Connection=True"
                : "User Id=sa;Password=" + conf.GetDbAdminUserPassword();

            return $"Server={server};Database={db};{auth};MultipleActiveResultSets=true;;{DBConnectionRetry}";
        }
    }
}
