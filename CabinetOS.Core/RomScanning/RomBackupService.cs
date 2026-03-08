using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class RomBackupService
    {
        private const string BackupFileName = "rom-backup.json";

        public void BackupPlatform(string platformRoot, IEnumerable<RomInfo> roms)
        {
            Directory.CreateDirectory(platformRoot);

            var dict = roms
                .Where(r => !string.IsNullOrWhiteSpace(r.OriginalName))
                .ToDictionary(
                    r => r.FileName,
                    r => r.OriginalName!,
                    StringComparer.OrdinalIgnoreCase);

            if (dict.Count == 0)
                return;

            var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(Path.Combine(platformRoot, BackupFileName), json);
        }

        public void RestorePlatform(string platformRoot)
        {
            var backupPath = Path.Combine(platformRoot, BackupFileName);
            if (!File.Exists(backupPath))
                return;

            var json = File.ReadAllText(backupPath);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (dict == null)
                return;

            foreach (var kvp in dict)
            {
                var currentPath = Path.Combine(platformRoot, kvp.Key);
                var originalPath = Path.Combine(platformRoot, kvp.Value);

                if (File.Exists(currentPath))
                    File.Move(currentPath, originalPath, overwrite: true);
            }
        }
    }
}