using System;
using System.IO;

namespace CabinetOS.Core.Settings
{
    public static class SettingsPaths
    {
        public static string Root =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CabinetOS");

        public static string SettingsFile => Path.Combine(Root, "settings.json");
        public static string SecretsFile => Path.Combine(Root, "secrets.json");
    }
}