using Microsoft.Extensions.Configuration;

namespace Gov.Jag.Embc.Public
{
    public static class ConfigurationEx
    {
        public static int ServerTimeoutInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("SESSION_TIMEOUT_MINUTES", 20);
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
    }
}
