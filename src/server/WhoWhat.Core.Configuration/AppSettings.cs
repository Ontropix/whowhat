using Platform.Common.AppSettings;

namespace WhoWhat.Core.Configuration
{
    public class AppSettings
    {
        [SettingsProperty("views_connection_string")]
        public string ViewConnectionString { get; set; }

        [SettingsProperty("logs_connection_string")]
        public string LogsConnectionString { get; set; }

        [SettingsProperty("events_connection_string")]
        public string EventsConnectionString { get; set; }

        [SettingsProperty("redis_connection_string")]
        public string RedisConnectionString { get; set; }

        [SettingsProperty("azure_connection_string")]
        public string AzureConnectionString { get; set; }
    }
}
