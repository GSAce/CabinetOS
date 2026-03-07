using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CabinetOS.Core.Arcade.Models;

namespace CabinetOS.Core.Arcade.Audit
{
    /// <summary>
    /// Performs a basic audit of ROMs and CHDs for a given DAT family.
    /// </summary>
    public static class ArcadeAuditService
    {
        /// <summary>
        /// Audits a DAT family against a source folder containing ROM archives and CHDs.
        /// </summary>
        /// <param name="family">DAT family name (e.g. "mame285").</param>
        /// <param name="sourceFolder">Root folder containing ROM archives and "chd" subfolder.</param>
        public static ArcadeAuditSummary AuditFamily(string family, string sourceFolder)
        {
            var games = DatManager.GetDat(family)
                ?? throw new InvalidOperationException(
                    $"No DAT loaded for family '{family}'. Ensure it exists in Arcade/DatFiles."
                );

            var summary = new ArcadeAuditSummary
            {
                Family = family,
                TotalGames = games.Count
            };

            // Pre-scan archives once to avoid repeated directory scans
            var archives = Directory.GetFiles(sourceFolder, "*.*")
                .Where(f =>
                {
                    var ext = Path.GetExtension(f).ToLowerInvariant();
                    return ext == ".zip" || ext == ".7z";
                })
                .ToList();

            foreach (var game in games)
            {
                bool gameMissingRoms = false;
                bool gameMissingChds = false;

                summary.TotalRoms += game.Roms.Count;
                summary.TotalChds += game.Disks.Count;

                // ROM audit
                foreach (var rom in game.Roms)
                {
                    if (!RomExistsInArchives(rom, archives))
                    {
                        summary.MissingRoms++;
                        gameMissingRoms = true;
                    }
                }

                // CHD audit
                foreach (var disk in game.Disks)
                {
                    string chdPath = Path.Combine(
                        sourceFolder,
                        "chd",
                        game.Name,
                        disk.Name
                    );

                    if (!File.Exists(chdPath))
                    {
                        summary.MissingChds++;
                        gameMissingChds = true;
                    }
                }

                if (!gameMissingRoms && !gameMissingChds)
                {
                    summary.GamesFullyPresent++;
                }
                else
                {
                    if (gameMissingRoms)
                        summary.GamesWithMissingRoms++;

                    if (gameMissingChds)
                        summary.GamesWithMissingChds++;

                    summary.IncompleteGames.Add(game.Name);
                }
            }

            return summary;
        }

        /// <summary>
        /// Checks whether a ROM exists in any of the given archives by filename.
        /// </summary>
        private static bool RomExistsInArchives(DatRomInfo rom, IEnumerable<string> archives)
        {
            // Very simple first pass: just check if the ROM name appears inside any archive.
            // You can later optimize this with a GlobalRomIndex or cached scan.
            foreach (var archivePath in archives)
            {
                using var stream = File.OpenRead(archivePath);
                using var reader = SharpCompress.Readers.ReaderFactory.OpenReader(stream);

                while (reader.MoveToNextEntry())
                {
                    var entry = reader.Entry;

                    if (entry.IsDirectory)
                        continue;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (entry.Key.Equals(rom.Name!, StringComparison.OrdinalIgnoreCase))
                        return true;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }

            return false;
        }
    }
}