namespace CabinetOS.Core.Settings.Models
{
    public class NetworkSettings
    {
        public bool EnableWifi { get; set; } = true;
        public string PreferredNetwork { get; set; } = "";
        public bool EnableDirectLink { get; set; } = false;
    }
}