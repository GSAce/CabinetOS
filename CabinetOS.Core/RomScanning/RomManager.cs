using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CabinetOS.Core.Models;
using CabinetOS.Core.Platforms.Detection;

namespace CabinetOS.Core.RomScanning
{
    public class RomManager
    {
        private readonly IRomScanner _scanner;
        private readonly RomNameNormalizer _normalizer;
        private readonly RomBackupService _backupService;
        private readonly PlatformRegistry _platformRegistry;

        public RomManager(
            IRomScanner scanner,
            RomNameNormalizer normalizer,
            RomBackupService backupService,
            PlatformRegistry platformRegistry)
        {
            _scanner = scanner;
            _normalizer = normalizer;
            _backupService = backupService;
            _platformRegistry = platformRegistry;
        }

        /// <summary>
        /// Full scan pipeline: scan → normalize → backup.
        /// </summary>
        public async Task<RomScanResult> ScanAndNormalizeAsync(
            string romRoot,
            bool normalizeNames,
            bool backupNames,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default)
        {
            // Step 1: Scan
            var scanResult = await _scanner.ScanAsync(romRoot, progress, cancellationToken);

            if (!normalizeNames && !backupNames)
                return scanResult;

            // Step 2: Normalize + Backup per platform
            foreach (var kvp in scanResult.Platforms)
            {
                var platformId = kvp.Key;
                var roms = kvp.Value;

                var platformDef = _platformRegistry.Platforms[platformId];
                var platformRoot = ResolvePlatformRoot(romRoot, platformDef);

                if (normalizeNames)
                {
                    foreach (var rom in roms)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        _normalizer.ApplyOnDisk(rom);
                    }
                }

                if (backupNames)
                {
                    _backupService.BackupPlatform(platformRoot, roms);
                }
            }

            return scanResult;
        }

        /// <summary>
        /// Scan only — no normalization, no backup.
        /// </summary>
        public Task<RomScanResult> ScanAsync(
            string romRoot,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default)
        {
            return _scanner.ScanAsync(romRoot, progress, cancellationToken);
        }

        /// <summary>
        /// Restore original filenames for all platforms.
        /// </summary>
        public void RestoreAll(string romRoot)
        {
            foreach (var platform in _platformRegistry.Platforms.Values)
            {
                var platformRoot = ResolvePlatformRoot(romRoot, platform);
                if (Directory.Exists(platformRoot))
                    _backupService.RestorePlatform(platformRoot);
            }
        }

        /// <summary>
        /// Restore original filenames for a single platform.
        /// </summary>
        public void RestorePlatform(string romRoot, string platformId)
        {
            if (!_platformRegistry.Platforms.TryGetValue(platformId, out var platform))
                return;

            var platformRoot = ResolvePlatformRoot(romRoot, platform);
            if (Directory.Exists(platformRoot))
                _backupService.RestorePlatform(platformRoot);
        }

        private string ResolvePlatformRoot(string romRoot, PlatformDefinition platform)
        {
            // Use the first folder name as the canonical root
            var folder = platform.FolderNames.FirstOrDefault();
            return folder == null
                ? romRoot
                : Path.Combine(romRoot, folder);
        }
    }
}