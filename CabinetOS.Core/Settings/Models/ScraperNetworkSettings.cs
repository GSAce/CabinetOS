namespace CabinetOS.Core.Settings.Models
{
    public class ScraperNetworkSettings
    {
        public int TimeoutSeconds { get; set; } = 30;
        public int RetryCount { get; set; } = 3;

        public bool UseProxy { get; set; } = false;
        public string ProxyAddress { get; set; } = "";
        public int ProxyPort { get; set; } = 0;

        public string UserAgent { get; set; } = "CabinetOS/1.0";
        public bool EnableCompression { get; set; } = true;

        public bool AllowInsecureSsl { get; set; } = false;
    }
}