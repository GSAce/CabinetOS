using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using CabinetOS.Core.Arcade.DatDiscovery;

namespace CabinetOS.Core.Arcade.Config
{
    public static class ArcadePlatformLoader
    {
        private static ArcadePlatformDefinition? _definition;
        private static bool _loaded;

        public static ArcadePlatformDefinition GetDefinition()
        {
            if (_loaded && _definition != null)
                return _definition;

            string baseDir = AppContext.BaseDirectory;
            string arcadeJsonPath = Path.Combine(baseDir, "Arcade", "arcade.json");
            string datFilesRoot = Path.Combine(baseDir, "Arcade", "DatFiles");

            if (!File.Exists(arcadeJsonPath))
                throw new FileNotFoundException("Arcade platform definition not found.", arcadeJsonPath);

            var json = File.ReadAllText(arcadeJsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var def = JsonSerializer.Deserialize<ArcadePlatformDefinition>(json, options)
                      ?? throw new InvalidOperationException("Failed to deserialize arcade.json");

            // Basic validation
            if (string.IsNullOrWhiteSpace(def.DefaultFamily))
                throw new InvalidOperationException("arcade.json: DefaultFamily must be set.");

            if (!def.Families.Contains(def.DefaultFamily, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("arcade.json: DefaultFamily must be listed in Families.");

            // Cross-check with DAT scanner
            var familiesOnDisk = ArcadeDatScanner.Scan(datFilesRoot)
                .Select(f => f.Family)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var missingFamilies = def.Families
                .Where(f => !familiesOnDisk.Contains(f))
                .ToList();

            if (missingFamilies.Count > 0)
            {
                // You can log this instead if you prefer non-fatal
                throw new InvalidOperationException(
                    "arcade.json: The following Families are not present on disk: " +
                    string.Join(", ", missingFamilies));
            }

            _definition = def;
            _loaded = true;
            return _definition;
        }
    }
}