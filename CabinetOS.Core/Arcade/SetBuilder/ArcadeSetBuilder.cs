using CabinetOS.Core.Arcade.Models;
using CabinetOS.Core.Arcade.SetBuilder;
using CabinetOS.Core.Utilities;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace CabinetOS.Core.Arcade
{
    public static class ArcadeSetBuilder
    {
        // ============================================================
        // BUILD SET (ENTRY POINT)
        // ============================================================
        public static BuildSummary BuildSet(BuildOptions options)
        {
            var sourceFolder = options.SourceFolder;
            var outputFolder = options.OutputFolder;
            var games = options.Games;
            var mode = options.Mode;
            var salvageIndex = options.SalvageIndex;
            var log = options.Log;
            var token = options.Cancellation;
            var parallel = options.Parallel;
            var dryRun = options.DryRun;

            var graph = BuildParentCloneGraph(games);

            int total = games.Count;
            int builtGames = 0;

            int missingRoms = 0;
            int salvagedRoms = 0;

            int totalChds = 0;
            int addedChds = 0;
            int salvagedChds = 0;
            int missingChds = 0;

            var summary = new BuildSummary
            {
                Log = log
            };

            var stopwatch = Stopwatch.StartNew();

            log?.Add($"Starting build: {total} games");

            // ------------------------------------------------------------
            // PARALLEL MODE
            // ------------------------------------------------------------
            if (parallel)
            {
                Parallel.ForEach(games, game =>
                {
                    token?.ThrowIfCancellationRequested();

                    int index = Interlocked.Increment(ref builtGames);
                    options.GameProgress?.Invoke(index, total, game.Name);
                    log?.Add($"Building {game.Name} (parallel)");

                    var deps = ResolveDependencies(game, graph, mode);

                    BuildZipForGame(
                        game, deps, sourceFolder, outputFolder,
                        dryRun,
                        salvageIndex,
                        options.RomProgress,
                        options.SalvageProgress,
                        () => Interlocked.Increment(ref missingRoms),
                        () => Interlocked.Increment(ref salvagedRoms),
                        summary,
                        log, token
                    );

                    CopyChdsForGame(
                        game, deps, sourceFolder, outputFolder,
                        dryRun,
                        salvageIndex,
                        options.ChdProgress,
                        options.SalvageProgress,
                        () => Interlocked.Increment(ref totalChds),
                        () => Interlocked.Increment(ref addedChds),
                        () => Interlocked.Increment(ref salvagedChds),
                        () => Interlocked.Increment(ref missingChds),
                        summary,
                        log, token
                    );
                });
            }
            else
            {
                // ------------------------------------------------------------
                // SEQUENTIAL MODE
                // ------------------------------------------------------------
                int index = 0;

                foreach (var game in games)
                {
                    token?.ThrowIfCancellationRequested();

                    index++;
                    options.GameProgress?.Invoke(index, total, game.Name);
                    log?.Add($"Building {game.Name}");

                    var deps = ResolveDependencies(game, graph, mode);

                    BuildZipForGame(
                        game, deps, sourceFolder, outputFolder,
                        dryRun,
                        salvageIndex,
                        options.RomProgress,
                        options.SalvageProgress,
                        () => missingRoms++,
                        () => salvagedRoms++,
                        summary,
                        log, token
                    );

                    CopyChdsForGame(
                        game, deps, sourceFolder, outputFolder,
                        dryRun,
                        salvageIndex,
                        options.ChdProgress,
                        options.SalvageProgress,
                        () => totalChds++,
                        () => addedChds++,
                        () => salvagedChds++,
                        () => missingChds++,
                        summary,
                        log, token
                    );

                    builtGames++;
                }
            }

            stopwatch.Stop();
            log?.Add("Build complete.");

            summary.TotalGames = total;
            summary.BuiltGames = builtGames;
            summary.MissingRoms = missingRoms;
            summary.SalvagedRoms = salvagedRoms;
            summary.TotalChds = totalChds;
            summary.AddedChds = addedChds;
            summary.SalvagedChds = salvagedChds;
            summary.MissingChds = missingChds;
            summary.ElapsedTime = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

            return summary;
        }

        // ============================================================
        // PARENT/CLONE GRAPH
        // ============================================================
        private static ParentCloneGraph BuildParentCloneGraph(List<DatGameInfo> games)
        {
            var graph = new ParentCloneGraph();

            foreach (var g in games)
                graph.Games[g.Name] = g;

            foreach (var g in games)
            {
                if (!string.IsNullOrEmpty(g.CloneOf))
                {
                    if (!graph.ClonesByParent.ContainsKey(g.CloneOf))
                        graph.ClonesByParent[g.CloneOf] = new List<DatGameInfo>();

                    graph.ClonesByParent[g.CloneOf].Add(g);
                }
            }

            return graph;
        }

        // ============================================================
        // DEPENDENCY RESOLUTION
        // ============================================================
        private static RomDependencyResult ResolveDependencies(
            DatGameInfo game,
            ParentCloneGraph graph,
            SetBuildMode mode)
        {
            var result = new RomDependencyResult();

            result.RequiredRoms.AddRange(game.Roms);
            result.RequiredDisks.AddRange(game.Disks);

            if (!string.IsNullOrEmpty(game.CloneOf) &&
                graph.Games.TryGetValue(game.CloneOf, out var parent))
            {
                if (mode == SetBuildMode.Merged || mode == SetBuildMode.Salvage)
                {
                    result.RequiredRoms.AddRange(parent.Roms);
                    result.RequiredDisks.AddRange(parent.Disks);
                }
            }

            return result;
        }

        // ============================================================
        // ZIP BUILDER
        // ============================================================
        private static void BuildZipForGame(
            DatGameInfo game,
            RomDependencyResult deps,
            string sourceFolder,
            string outputFolder,
            bool dryRun,
            GlobalRomIndex? salvageIndex,
            Action<string, string>? romProgress,
            Action<string>? salvageProgress,
            Action? onMissingRom,
            Action? onSalvagedRom,
            BuildSummary summary,
            BuildLog? log,
            CancellationToken? token)
        {
            string outputZipPath = Path.Combine(outputFolder, $"{game.Name}.zip");
            Directory.CreateDirectory(outputFolder);

            ZipArchive? zip = null;
            FileStream? zipStream = null;

            if (!dryRun)
            {
                zipStream = File.Create(outputZipPath);
                zip = new ZipArchive(zipStream, ZipArchiveMode.Create);
            }

            try
            {
                foreach (var rom in deps.RequiredRoms)
                {
                    token?.ThrowIfCancellationRequested();

                    romProgress?.Invoke(game.Name, rom.Name);
                    log?.Add($"ROM: {rom.Name}");

                    AddRomToZip(
                        zip, game, rom, sourceFolder, dryRun, salvageIndex,
                        salvageProgress, onMissingRom, onSalvagedRom,
                        summary, log, token
                    );
                }
            }
            finally
            {
                zip?.Dispose();
                zipStream?.Dispose();
            }
        }

        // ============================================================
        // ADD ROM TO ZIP (WITH SALVAGE + LOGGING + SUMMARY + DRY RUN)
        // ============================================================
        private static void AddRomToZip(
            ZipArchive? zip,
            DatGameInfo game,
            DatRomInfo rom,
            string sourceFolder,
            bool dryRun,
            GlobalRomIndex? salvageIndex,
            Action<string>? salvageProgress,
            Action? onMissingRom,
            Action? onSalvagedRom,
            BuildSummary summary,
            BuildLog? log,
            CancellationToken? token)
        {
            token?.ThrowIfCancellationRequested();

            // 1) Try normal source folder first
            if (TryAddRomFromSource(zip, Path.Combine(sourceFolder, rom.Name), rom.Name, dryRun))
                return;

            // 2) Mark missing
            onMissingRom?.Invoke();
            log?.Add($"MISSING: {rom.Name}");

            lock (summary)
            {
                summary.MissingRoms++;
                summary.MissingRomList.Add($"{game.Name}/{rom.Name}");
            }

            if (salvageIndex == null)
                return;

            salvageProgress?.Invoke(rom.Name);
            log?.Add($"SALVAGE: Attempting {rom.Name}");

            // ------------------------------------------------------------
            // SHA1 salvage
            // ------------------------------------------------------------
            if (!string.IsNullOrEmpty(rom.SHA1) &&
                salvageIndex.BySha1.TryGetValue(rom.SHA1, out var sha1Archive))
            {
                if (TryAddRomFromSource(zip, sha1Archive, rom.Name, dryRun))
                {
                    onSalvagedRom?.Invoke();
                    log?.Add($"SALVAGED: {rom.Name}");

                    lock (summary)
                    {
                        summary.SalvagedRoms++;
                    }
                }
                return;
            }

            // ------------------------------------------------------------
            // CRC salvage
            // ------------------------------------------------------------
            if (!string.IsNullOrEmpty(rom.CRC) &&
                salvageIndex.ByCrc.TryGetValue(rom.CRC, out var crcArchive))
            {
                if (TryAddRomFromSource(zip, crcArchive, rom.Name, dryRun))
                {
                    onSalvagedRom?.Invoke();
                    log?.Add($"SALVAGED: {rom.Name}");

                    lock (summary)
                    {
                        summary.SalvagedRoms++;
                    }
                }
                return;
            }

            // ------------------------------------------------------------
            // MD5 salvage
            // ------------------------------------------------------------
            if (!string.IsNullOrEmpty(rom.MD5) &&
                salvageIndex.ByMd5.TryGetValue(rom.MD5, out var md5Archive))
            {
                if (TryAddRomFromSource(zip, md5Archive, rom.Name, dryRun))
                {
                    onSalvagedRom?.Invoke();
                    log?.Add($"SALVAGED: {rom.Name}");

                    lock (summary)
                    {
                        summary.SalvagedRoms++;
                    }
                }
                return;
            }
        }

        // ============================================================
        // TRY ADD ROM FROM SOURCE ARCHIVES (DRY RUN AWARE)
        // ============================================================
        private static bool TryAddRomFromSource(
            ZipArchive? targetZip,
            string sourceArchivePath,
            string romFileName,
            bool dryRun)
        {
            if (!File.Exists(sourceArchivePath))
                return false;

            using var stream = File.OpenRead(sourceArchivePath);
            using var reader = ReaderFactory.OpenReader(stream);

            while (reader.MoveToNextEntry())
            {
                var entry = reader.Entry;

                if (entry.IsDirectory)
                    continue;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (!entry.Key.Equals(romFileName, StringComparison.OrdinalIgnoreCase))
                    continue;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                if (!dryRun && targetZip != null)
                {
                    using var entryStream = reader.OpenEntryStream();
                    var newEntry = targetZip.CreateEntry(romFileName, CompressionLevel.Optimal);

                    using var outStream = newEntry.Open();
                    entryStream.CopyTo(outStream);
                }

                return true;
            }

            return false;
        }

        // ============================================================
        // ADD CHD (WITH SALVAGE + LOGGING + SUMMARY + DRY RUN)
        // ============================================================
        private static void AddChd(
            string targetFolder,
            DatGameInfo game,
            DatDiskInfo disk,
            string sourceChdRoot,
            bool dryRun,
            GlobalRomIndex? salvageIndex,
            Action<string>? salvageProgress,
            Action? onMissingChd,
            Action? onSalvagedChd,
            Action? onAddedChd,
            BuildSummary summary,
            BuildLog? log,
            CancellationToken? token)
        {
            token?.ThrowIfCancellationRequested();

            var expectedPath = Path.Combine(sourceChdRoot, disk.Name);
            var destPath = Path.Combine(targetFolder, disk.Name);

            // 1) Try normal source folder
            if (File.Exists(expectedPath))
            {
                Directory.CreateDirectory(targetFolder);

                if (!dryRun)
                    File.Copy(expectedPath, destPath, overwrite: true);

                // In dry run, we skip SHA1 verification (no file written)
                if (!dryRun && !VerifyChdSha1(destPath, disk.SHA1))
                {
                    log?.Add($"CHD SHA1 FAIL: {disk.Name}");
                    File.Delete(destPath);
                }
                else
                {
                    onAddedChd?.Invoke();
                    lock (summary)
                    {
                        summary.AddedChds++;
                    }
                    return;
                }
            }

            // 2) Mark missing
            onMissingChd?.Invoke();
            log?.Add($"MISSING CHD: {disk.Name}");

            lock (summary)
            {
                summary.MissingChds++;
                summary.MissingChdList.Add($"{game.Name}/{disk.Name}");
            }

            if (salvageIndex == null)
                return;

            salvageProgress?.Invoke(disk.Name);
            log?.Add($"SALVAGE CHD: Attempting {disk.Name}");

            // 3) Try salvage by CHD filename
            if (salvageIndex.ByChdName.TryGetValue(disk.Name, out var altChdPath))
            {
                if (File.Exists(altChdPath))
                {
                    Directory.CreateDirectory(targetFolder);

                    if (!dryRun)
                        File.Copy(altChdPath, destPath, overwrite: true);

                    if (!dryRun && !VerifyChdSha1(destPath, disk.SHA1))
                    {
                        log?.Add($"CHD SHA1 FAIL (SALVAGED): {disk.Name}");
                        File.Delete(destPath);
                        return;
                    }

                    onSalvagedChd?.Invoke();
                    log?.Add($"SALVAGED CHD: {disk.Name}");

                    lock (summary)
                    {
                        summary.SalvagedChds++;
                    }
                }
            }
        }

        // ============================================================
        // CHD COPYING (NOW SALVAGE-AWARE + DRY RUN AWARE)
        // ============================================================
        private static void CopyChdsForGame(
            DatGameInfo game,
            RomDependencyResult deps,
            string sourceFolder,
            string outputFolder,
            bool dryRun,
            GlobalRomIndex? salvageIndex,
            Action<string, string>? chdProgress,
            Action<string>? salvageProgress,
            Action? onTotalChd,
            Action? onAddedChd,
            Action? onSalvagedChd,
            Action? onMissingChd,
            BuildSummary summary,
            BuildLog? log,
            CancellationToken? token)
        {
            string chdOutputDir = Path.Combine(outputFolder, "chd", game.Name);
            Directory.CreateDirectory(chdOutputDir);

            string sourceChdRoot = Path.Combine(sourceFolder, "chd", game.Name);

            foreach (var disk in deps.RequiredDisks)
            {
                token?.ThrowIfCancellationRequested();

                onTotalChd?.Invoke();

                chdProgress?.Invoke(game.Name, disk.Name);
                log?.Add($"CHD: {disk.Name}");

                AddChd(
                    chdOutputDir,
                    game,
                    disk,
                    sourceChdRoot,
                    dryRun,
                    salvageIndex,
                    salvageProgress,
                    onMissingChd,
                    onSalvagedChd,
                    onAddedChd,
                    summary,
                    log,
                    token
                );
            }
        }

        // ============================================================
        // CHD SHA1 VERIFICATION
        // ============================================================
        private static bool VerifyChdSha1(string filePath, string? expectedSha1)
        {
            try
            {
                using var stream = File.OpenRead(filePath);
                string sha1 = HashService.ComputeSHA1(stream);
                return sha1.Equals(expectedSha1, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}