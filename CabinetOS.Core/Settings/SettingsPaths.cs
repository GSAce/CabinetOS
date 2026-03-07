using System.IO;

namespace CabinetOS.Core.Settings
{
    public static class SettingsPaths
    {
        public static string BaseFolder =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CabinetOS");

        public static string GeneralSettingsFile => Path.Combine(BaseFolder, "general.json");
        public static string AppearanceSettingsFile => Path.Combine(BaseFolder, "appearance.json");
        public static string NetworkSettingsFile => Path.Combine(BaseFolder, "network.json");
        public static string PathsSettingsFile => Path.Combine(BaseFolder, "paths.json");
        public static string ScraperSettingsFile => Path.Combine(BaseFolder, "scraper.json");
        public static string AdvancedSettingsFile => Path.Combine(BaseFolder, "advanced.json");
    }
}