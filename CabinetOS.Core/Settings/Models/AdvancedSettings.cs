namespace CabinetOS.Core.Settings.Models
{
    public class AdvancedSettings
    {
        public bool EnableDebugLogging { get; set; } = false;
        public bool ShowDeveloperOptions { get; set; } = false;
        public bool EnableExperimentalFeatures { get; set; } = false;
    }
}