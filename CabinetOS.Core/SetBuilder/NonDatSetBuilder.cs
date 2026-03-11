using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CabinetOS.Core.Models;
using CabinetOS.Core.Platforms.Detection;
using CabinetOS.Core.RomScanning;

namespace CabinetOS.Core.SetBuilder
{
    public class NonDatSetBuilder
    {
        private readonly RomManager _romManager;
        private readonly PlatformRegistry _platformRegistry;

        public NonDatSetBuilder(RomManager romManager, PlatformRegistry platformRegistry)
        {
            _romManager = romManager;
            _platformRegistry = platformRegistry;
        }

        public async Task<NonDatBuildSummary> BuildAsync(
            NonDatBuildOptions options,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default)
        {
            ValidateOptions(options);

            // 1. Scan (and optionally normalize + backup)
            var scanResult = await _romManager.ScanAndNormalizeAsync(
                options.RomRoot,
                options.NormalizeNames,
                options.BackupNames,
                progress: null,
                cancellationToken: cancellationToken);

            // 2. Filter platforms
            var platformsToBuild = FilterPlatforms(scanResult, options);

            var summary = new NonDatBuildSummary
            {
                TotalPlatforms = platformsToBuild.Count
            };

            Directory.CreateDirectory(options.OutputRoot);

            var regionSelector = new RegionSelector(options.RegionPreference);
            var discGroupDetector = new DiscGroupDetector();
            var m3uGenerator = new M3UGenerator();

            // 3. Build per platform
            var totalPlatforms = platformsToBuild.Count;
            var processedPlatforms = 0;

            foreach (var kvp in platformsToBuild)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var platformId = kvp.Key;
                var roms = kvp.Value;

                if (!_platformRegistry.Platforms.TryGetValue(platformId, out var platformDef))
                {
                    summary.Warnings.Add($"Platform '{platformId}' not found in registry.");
                    continue;
                }

                var platformOutput = Path.Combine(options.OutputRoot, platformDef.ShortName ?? platformDef.Id);
                Directory.CreateDirectory(platformOutput);

                await BuildPlatformAsync(
                    platformId,
                    roms,
                    platformOutput,
                    options,
                    summary,
                    regionSelector,
                    discGroupDetector,
                    m3uGenerator,
                    cancellationToken);

                processedPlatforms++;
                progress?.Report((double)processedPlatforms / totalPlatforms);
            }

            return summary;
        }

        private void ValidateOptions(NonDatBuildOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.RomRoot))
                throw new ArgumentException("RomRoot must be provided.", nameof(options.RomRoot));

            if (string.IsNullOrWhiteSpace(options.OutputRoot))
                throw new ArgumentException("OutputRoot must be provided.", nameof(options.OutputRoot));
        }

        private Dictionary<string, List<RomInfo>> FilterPlatforms(
            RomScanResult scanResult,
            NonDatBuildOptions options)
        {
            if (options.IncludedPlatforms == null || options.IncludedPlatforms.Count == 0)
                return scanResult.Platforms;

            var set = new HashSet<string>(options.IncludedPlatforms, StringComparer.OrdinalIgnoreCase);
            return scanResult.Platforms
                .Where(kvp => set.Contains(kvp.Key))
                .ToDictionary(k => k.Key, v => v.Value);
        }

        private async Task BuildPlatformAsync(
            string platformId,
            List<RomInfo> roms,
            string outputRoot,
            NonDatBuildOptions options,
            NonDatBuildSummary summary,
            RegionSelector regionSelector,
            DiscGroupDetector discGroupDetector,
            M3UGenerator m3uGenerator,
            CancellationToken cancellationToken)
        {
            // Group by game key
            var groups = discGroupDetector.GroupByGame(roms);

            foreach (var group in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var gameKey = group.Key;
                var gameRoms = group.Value;

                // Region selection
                var selectedRoms = regionSelector.SelectBestRegion(gameRoms);

                // Multi-disc grouping
                if (options.GroupMultiDisc && selectedRoms.Any(r => r.IsMultiDisc))
                {
                    var folderName = SanitizeFolderName(gameKey);
                    var gameFolder = Path.Combine(outputRoot, folderName);
                    Directory.CreateDirectory(gameFolder);

                    foreach (var rom in selectedRoms)
                    {
                        await CopyRomAsync(rom, gameFolder, options, summary, cancellationToken);
                    }

                    // Generate M3U playlist for multi-disc sets
                    m3uGenerator.Generate(gameFolder, folderName, selectedRoms);
                }
                else
                {
                    foreach (var rom in selectedRoms)
                    {
                        await CopyRomAsync(rom, outputRoot, options, summary, cancellationToken);
                    }
                }

                summary.TotalGames++;
            }
        }

        private async Task CopyRomAsync(
            RomInfo rom,
            string targetFolder,
            NonDatBuildOptions options,
            NonDatBuildSummary summary,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (rom.IsBios && !options.IncludeBios)
                return;

            if (rom.IsChd)
            {
                if (!options.IncludeChd)
                    return;

                summary.TotalChds++;
            }

            Directory.CreateDirectory(targetFolder);

            var targetPath = Path.Combine(targetFolder, rom.FileName);

            using (var source = File.OpenRead(rom.FilePath))
            using (var dest = File.Create(targetPath))
            {
                await source.CopyToAsync(dest, cancellationToken);
            }

            var fileInfo = new FileInfo(targetPath);
            summary.TotalBytes += fileInfo.Length;
            summary.TotalRoms++;
        }

        private string SanitizeFolderName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var cleaned = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
            return string.IsNullOrWhiteSpace(cleaned) ? "Game" : cleaned;
        }
    }
}