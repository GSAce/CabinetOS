using CabinetOS.Core.Arcade.Config;
using CabinetOS.Core.Arcade.Filtering;
using CabinetOS.Core.Arcade.Models;
using CabinetOS.Core.Arcade.RomScanner;
using CabinetOS.Core.Arcade.SetBuilder;
using System;
using System.Threading;
using static CabinetOS.Core.Settings.Models.GeneralSettings; // for HashScanMode

namespace CabinetOS.Core.Arcade.Orchestration
{
    /// <summary>
    /// High-level coordinator for scanning + filtering + building an arcade set.
    /// </summary>
    public static class ArcadeBuildOrchestrator
    {
        public static BuildSummary BuildArcadeSet(
            string sourceFolder,
            string outputFolder,
            string? familyOverride,
            ArcadeFilterOptions? filterOptions,
            bool dryRun = false,   // ← NEW: dry run toggle
            Action<string>? onStatus = null,
            Action<string, string>? onRomFound = null,
            Action<string>? onChdFound = null)
        {
            // ------------------------------------------------------------
            // 1) Load arcade.json
            // ------------------------------------------------------------
            var def = ArcadePlatformLoader.GetDefinition();

            // ------------------------------------------------------------
            // 2) Determine DAT family
            // ------------------------------------------------------------
            var family = string.IsNullOrWhiteSpace(familyOverride)
                ? def.DefaultFamily
                : familyOverride;

            // ------------------------------------------------------------
            // 3) Load games from DatManager
            // ------------------------------------------------------------
            var games = DatManager.GetDat(family)
                ?? throw new InvalidOperationException(
                    $"No DAT loaded for family '{family}'."
                );

            // ------------------------------------------------------------
            // 4) Apply filters
            // ------------------------------------------------------------
            if (filterOptions != null)
                games = ArcadeGameFilter.ApplyFilters(games, filterOptions);

            // ------------------------------------------------------------
            // 5) Scan ROM directory (build GlobalRomIndex)
            // ------------------------------------------------------------
            onStatus?.Invoke("Scanning ROM directory...");

            var scan = ArcadeRomScannerService.Scan(
                sourceFolder,
                games,
                HashScanMode.Auto,   // Auto for now; UI can expose Fast/Deep later
                onStatus,
                onRomFound,
                onChdFound
            );

            // ------------------------------------------------------------
            // 6) Map merge mode
            // ------------------------------------------------------------
            var mode = def.MergeMode?.ToLowerInvariant() switch
            {
                "merged" => SetBuildMode.Merged,
                "nonmerged" => SetBuildMode.NonMerged,
                "salvage" => SetBuildMode.Salvage,
                _ => SetBuildMode.Split
            };

            // ------------------------------------------------------------
            // 7) BuildOptions
            // ------------------------------------------------------------
            var options = new BuildOptions
            {
                SourceFolder = sourceFolder,
                OutputFolder = outputFolder,
                Games = games,
                Mode = mode,
                SalvageIndex = scan.Index,
                Log = new BuildLog(),
                Cancellation = CancellationToken.None,
                Parallel = true,
                DryRun = dryRun,   // ← NEW: pass dry run flag

                // Progress callbacks
                GameProgress = (i, total, name) =>
                    onStatus?.Invoke($"[{i}/{total}] {name}"),

                RomProgress = onRomFound,
                ChdProgress = (game, chd) =>
                    onChdFound?.Invoke(chd),

                SalvageProgress = rom =>
                    onStatus?.Invoke($"Salvaging {rom}...")
            };

            // ------------------------------------------------------------
            // 8) Build set
            // ------------------------------------------------------------
            onStatus?.Invoke(dryRun ? "Simulating build..." : "Building set...");
            return ArcadeSetBuilder.BuildSet(options);
        }
    }
}