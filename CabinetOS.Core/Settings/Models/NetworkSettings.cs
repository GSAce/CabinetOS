namespace CabinetOS.Core.Settings.Models
{
    public class NetworkSettings
    {
        // --- DirectLink (Primary Connection) ---
        public bool DirectLinkEnabled { get; set; } = true;

        // Static IPs for PC ↔ Pi communication
        public string PcIPAddress { get; set; } = "10.0.0.1";
        public string PiIPAddress { get; set; } = "10.0.0.2";
        public string SubnetMask { get; set; } = "255.255.255.0";

        // WiFi Direct configuration (Pi uses its WiFi adapter)
        public string DirectLinkSSID { get; set; } = "CabinetOS-DirectLink";
        public string DirectLinkPassword { get; set; } = "cabinetos123";
        public int DirectLinkChannel { get; set; } = 6;

        // --- Ethernet Fallback (PC only) ---
        public bool UseEthernetForInternet { get; set; } = true;

        // --- Pi Internet Access (usually disabled) ---
        public bool AllowPiInternetAccess { get; set; } = false;

        // --- Runtime Status (not saved) ---
        [System.Text.Json.Serialization.JsonIgnore]
        public string LastKnownPiIP { get; set; } = "";

        [System.Text.Json.Serialization.JsonIgnore]
        public string LastConnectionStatus { get; set; } = "Unknown";

        [System.Text.Json.Serialization.JsonIgnore]
        public string LastError { get; set; } = "";
    }
}