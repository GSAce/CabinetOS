using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CabinetOS.Core.Arcade.RomScanner.Hashing
{
    public class HashCache
    {
        public Dictionary<string, HashCacheEntry> Entries { get; } =
            new(StringComparer.OrdinalIgnoreCase);

        public static HashCache Load(string path)
        {
            if (!File.Exists(path))
                return new HashCache();

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<HashCache>(json) ?? new HashCache();
        }

        public void Save(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json);
        }
    }
}