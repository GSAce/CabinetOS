using System;
using System.IO;
using System.Text.Json;

namespace CabinetOS.Core.Config.Settings
{
    public static class SettingsService
    {
        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "settings.json");

        private static AppSettings? _cached;

        public static AppSettings Load()
        {
            if (_cached != null)
                return _cached;

            if (!File.Exists(SettingsPath))
            {
                _cached = new AppSettings();
                Save(_cached);
                return _cached;
            }

            var json = File.ReadAllText(SettingsPath);
            _cached = JsonSerializer.Deserialize<AppSettings>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new AppSettings();

            return _cached;
        }

        public static void Save(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(SettingsPath, json);
            _cached = settings;
        }
    }
}