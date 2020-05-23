using System.Security.Policy;

namespace Gov.Jag.Embc.Public.ViewModels
{
    public class ConfigurationInfo
    {
        /// <summary>
        /// Base Path of the application
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Base URI for the application
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// Dotnet Environment (Development, Staging, Production...)
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Environment Title (an HTML string)
        /// </summary>
        public string EnvironmentTitle { get; set; }

        /// <summary>
        /// File creation time for the running assembly
        /// </summary>
        public string FileCreationTime { get; set; }

        /// <summary>
        /// File version for the running assembly
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// Git commit used to build the application
        /// </summary>
        public string SourceCommit { get; set; }

        /// <summary>
        /// Git reference used to build the application
        /// </summary>
        public string SourceReference { get; set; }

        /// <summary>
        /// Git repository used to build the application
        /// </summary>
        public string SourceRepository { get; set; }

        public int ClientTimeoutWarningInMinutes { get; internal set; }
        public int DefaultTimeoutWarningInMinutes { get; internal set; }
        public int ClientTimeoutWarningDurationInMinutes { get; internal set; }
        public int DefaultWarningDurationInMinutes { get; internal set; }
    }
}
