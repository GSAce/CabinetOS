using System.Text.Json;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.Services
{
    public static class PlatformDatabase
    {
        public static List<PlatformInfo> Platforms { get; private set; } = new();

        public static bool _isLoaded = false;

        private static readonly string PlatformFilePath =
            Path.Combine(AppContext.BaseDirectory, "PlatformData", "platforms.json");

        public static void Load()
        {
            if (!File.Exists(PlatformFilePath))
                throw new FileNotFoundException("platforms.json not found", PlatformFilePath);

            string json = File.ReadAllText(PlatformFilePath);

            Platforms = JsonSerializer.Deserialize<List<PlatformInfo>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PlatformInfo>();
        }
        public static void EnsureLoaded()
        {
            if (_isLoaded)
                return;

            Load();
            _isLoaded = true;
        }
        public static PlatformInfo? GetPlatformByExtension(string extension)
        {
            extension = extension.ToLower();

            return Platforms.FirstOrDefault(p =>
                p.Extensions.Any(ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)));
        }
    }
}