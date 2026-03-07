using CabinetOS.Core.Arcade.Models;
using CabinetOS.Core.Arcade.RomScanner.Hashing;
using CabinetOS.Core.Arcade.SetBuilder;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CabinetOS.Core.Settings.Models.GeneralSettings;

namespace CabinetOS.Core.Arcade.RomScanner
{
    /// <summary>
    /// Scans the ROM source folder and builds a GlobalRomIndex.
    /// Also detects missing ROMs and CHDs for the given DAT family.
    /// </summary>
    public static class ArcadeRomScannerService
    {
        public static ArcadeRomScanResult Scan(
            string sourceFolder,
            IEnumerable<DatGameInfo> games,
            HashScanMode hashMode,
            Action<string>? onStatus = null,
            Action<string, string>? onRomFound = null,
            Action<string>? onChdFound = null)
        {
            var result = new ArcadeRomScanResult();

            // ------------------------------------------------------------
            // Load hash cache
            // ------------------------------------------------------------
            string cachePath = Path.Combine(sourceFolder, "hashcache.json");
            var hashCache = HashCache.Load(cachePath);

            onStatus?.Invoke("Scanning archives...");

            // ------------------------------------------------------------
            // 1. Scan all .zip and .7z archives once
            // ------------------------------------------------------------
            var archives = Directory.GetFiles(sourceFolder, "*.*")
                .Where(f =>
                {
                    var ext = Path.GetExtension(f).ToLowerInvariant();
                    return ext == ".zip" || ext == ".7z";
                })
                .ToList();

            result.TotalArchives = archives.Count;

            foreach (var archivePath in archives)
            {
                using var stream = File.OpenRead(archivePath);
                using var reader = ReaderFactory.OpenReader(stream);

                while (reader.MoveToNextEntry())
                {
                    var entry = reader.Entry;

                    if (entry.IsDirectory)
                        continue;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string romName = entry.Key;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8604 // Possible null reference argument.
                    result.Index.ByName[romName] = archivePath;
#pragma warning restore CS8604 // Possible null reference argument.

                    result.TotalRoms++;
                    onRomFound?.Invoke(Path.GetFileName(archivePath), romName);
                }

                // ------------------------------------------------------------
                // Optional hashing (Deep / Auto / Fast)
                // ------------------------------------------------------------
                if (ShouldHash(result.SalvageModeEnabled))
                {
                    var hash = HashingHelpers.GetOrComputeHash(archivePath, hashCache);

                    if (hash.SHA1 != null)
                        result.Index.BySha1[hash.SHA1] = archivePath;

                    if (hash.MD5 != null)
                        result.Index.ByMd5[hash.MD5] = archivePath;

                    if (hash.CRC != null)
                        result.Index.ByCrc[hash.CRC] = archivePath;
                }
            }

            // ------------------------------------------------------------
            // 2. Scan CHDs
            // ------------------------------------------------------------
            onStatus?.Invoke("Scanning CHDs...");

            foreach (var game in games)
            {
                foreach (var disk in game.Disks)
                {
                    string chdPath = Path.Combine(
                        sourceFolder,
                        "chd",
                        game.Name,
                        disk.Name
                    );

                    if (File.Exists(chdPath))
                    {
                        if (ShouldHash(result.SalvageModeEnabled))
                        {
                            var hash = HashingHelpers.GetOrComputeHash(chdPath, hashCache);

                            if (hash.SHA1 != null)
                                result.Index.BySha1[hash.SHA1] = chdPath;

                            if (hash.MD5 != null)
                                result.Index.ByMd5[hash.MD5] = chdPath;

                            if (hash.CRC != null)
                                result.Index.ByCrc[hash.CRC] = chdPath;
                        }

                        result.Index.ByChdName[disk.Name] = chdPath;
                        result.TotalChds++;
                        onChdFound?.Invoke(disk.Name);
                    }
                }
            }

            // ------------------------------------------------------------
            // 3. Detect missing ROMs and CHDs
            // ------------------------------------------------------------
            onStatus?.Invoke("Checking for missing ROMs...");

            foreach (var game in games)
            {
                foreach (var rom in game.Roms)
                {
                    if (!result.Index.ByName.ContainsKey(rom.Name))
                        result.MissingRoms.Add($"{game.Name}/{rom.Name}");
                }

                foreach (var disk in game.Disks)
                {
                    if (!result.Index.ByChdName.ContainsKey(disk.Name))
                        result.MissingChds.Add($"{game.Name}/{disk.Name}");
                }
            }

            onStatus?.Invoke("ROM scan complete.");

            // ------------------------------------------------------------
            // Save hash cache
            // ------------------------------------------------------------
            hashCache.Save(cachePath);

            return result;

            // ------------------------------------------------------------
            // Local helper: determines whether hashing should occur
            // ------------------------------------------------------------
            bool ShouldHash(bool isSalvageMode)
            {
                return hashMode switch
                {
                    HashScanMode.Fast => false,
                    HashScanMode.Deep => true,
                    HashScanMode.Auto => isSalvageMode,
                    _ => false
                };
            }
        }
    }
}