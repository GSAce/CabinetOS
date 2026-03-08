using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CabinetOS.Core.Models;
using CabinetOS.Core.Platforms.Detection;

namespace CabinetOS.Core.RomScanning
{
    public class NonDatScanner : IRomScanner
    {
        private readonly PlatformRegistry _platformRegistry;

        public NonDatScanner(PlatformRegistry platformRegistry)
        {
            _platformRegistry = platformRegistry;
        }

        public async Task<RomScanResult> ScanAsync(
            string romRoot,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(romRoot))
                throw new ArgumentException("ROM root path must be provided.", nameof(romRoot));

            var result = new RomScanResult();
            var platforms = _platformRegistry.Platforms.Values.ToList();
            if (platforms.Count == 0)
                return result;

            var dict = new ConcurrentDictionary<string, List<RomInfo>>();
            var total = platforms.Count;
            var processed = 0;

            await Task.Run(() =>
            {
                Parallel.ForEach(platforms, new ParallelOptions
                {
                    CancellationToken = cancellationToken
                }, platform =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var roms = ScanPlatform(romRoot, platform, cancellationToken);
                    if (roms.Count > 0)
                        dict[platform.Id] = roms;

                    var done = Interlocked.Increment(ref processed);
                    progress?.Report((double)done / total);
                });
            }, cancellationToken);

            result.Platforms = dict
                .OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(k => k.Key, v => v.Value);

            result.TotalFiles = result.Platforms.Sum(kvp => kvp.Value.Count);
            return result;
        }

        private List<RomInfo> ScanPlatform(
            string romRoot,
            PlatformDefinition platform,
            CancellationToken cancellationToken)
        {
            var list = new List<RomInfo>();

            foreach (var folderName in platform.FolderNames)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var folderPath = Path.Combine(romRoot, folderName);
                if (!Directory.Exists(folderPath))
                    continue;

                var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (!platform.Extensions.Contains(ext))
                        continue;

                    var fileName = Path.GetFileName(file);

                    var rom = new RomInfo
                    {
                        FilePath = file,
                        FileName = fileName,
                        Extension = ext,
                        Platform = new PlatformInfo
                        {
                            Id = platform.Id,
                            Name = platform.Name
                        }
                    };

                    ClassifyRom(rom);
                    list.Add(rom);
                }
            }

            return list;
        }

        private void ClassifyRom(RomInfo rom)
        {
            // CHD detection
            if (rom.Extension == ".chd")
                rom.IsChd = true;

            // BIOS detection (simple heuristic)
            if (rom.FileName.Contains("bios", StringComparison.OrdinalIgnoreCase))
                rom.IsBios = true;

            // Region detection
            rom.Region = DetectRegionFromName(rom.FileName);

            // Multi-disc detection
            if (TryDetectDiscId(rom.FileName, out var discId))
            {
                rom.DiscId = discId;
                rom.IsMultiDisc = true;
            }
        }

        private string? DetectRegionFromName(string fileName)
        {
            var lower = fileName.ToLowerInvariant();

            if (lower.Contains("(usa)") || lower.Contains("(u)"))
                return "USA";
            if (lower.Contains("(europe)") || lower.Contains("(e)"))
                return "Europe";
            if (lower.Contains("(japan)") || lower.Contains("(j)"))
                return "Japan";
            if (lower.Contains("(world)"))
                return "World";

            return null;
        }

        private bool TryDetectDiscId(string fileName, out string? discId)
        {
            discId = null;
            var lower = fileName.ToLowerInvariant();

            var markers = new[]
            {
                "(disc 1)", "(disc 2)", "(disc 3)", "(disc 4)",
                "(cd1)", "(cd2)", "(cd3)", "(cd4)",
                "(disk 1)", "(disk 2)", "(disk 3)", "(disk 4)"
            };

            foreach (var marker in markers)
            {
                if (lower.Contains(marker))
                {
                    discId = marker.Trim('(', ')');
                    return true;
                }
            }

            return false;
        }
    }
}