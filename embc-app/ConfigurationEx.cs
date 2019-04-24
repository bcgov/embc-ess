using Microsoft.Extensions.Configuration;

namespace Gov.Jag.Embc.Public
{
    public static class ConfigurationEx
    {
        public static int ServerTimeoutInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("SESSION_TIMEOUT_MINUTES", 20);
        }

        public static int ClientTimeoutWarningDurationInMinutes(this IConfiguration conf)
        {
            return conf.GetValue("CLIENT_TIMEOUT_WARNING_MINUTES", 4);
        }

        public static int ClientTimeoutWarningInMinutes(this IConfiguration conf)
        {
            return conf.ServerTimeoutInMinutes() - conf.ClientTimeoutWarningDurationInMinutes() - 1;
        }
    }
}
