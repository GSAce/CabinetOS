using CabinetOS.Core.Platforms;
using CabinetOS.Core.Platforms.Detection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CabinetOS.Core.Metadata
{
    public static class MetadataService
    {
        private static string _romRoot = string.Empty;
        private static PlatformRegistry? _registry;

        public static void Initialize(string romRoot)
        {
            _romRoot = romRoot;

            if (_registry == null)
            {
                string platformJson = Path.Combine(AppContext.BaseDirectory, "PlatformData", "platforms.json");
                _registry = new PlatformRegistry(platformJson);
                _registry.DumpDebugInfo();
            }
        }

        // ---------------------------------------------------------
        // NORMALIZATION
        // ---------------------------------------------------------
        private static string Normalize(string s)
        {
            return s
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("_", "");
        }

        // ---------------------------------------------------------
        // PLATFORM LOOKUP
        // ---------------------------------------------------------
        private static PlatformDefinition? GetPlatformByFolder(string folderName)
        {
            if (_registry == null)
                return null;

            string normalized = Normalize(folderName);
            Debug.WriteLine($"[Metadata] LOOKUP: '{folderName}' → '{normalized}'");

            foreach (var p in _registry.Platforms.Values)
            {
                foreach (var fn in p.FolderNames)
                {
                    string fnNorm = Normalize(fn);
                    if (fnNorm == normalized)
                    {
                        Debug.WriteLine($"[Metadata] MATCH: Folder '{folderName}' → Platform '{p.Id}'");
                        return p;
                    }
                }
            }

            Debug.WriteLine($"[Metadata] NO MATCH: '{folderName}' has no platform definition");
            return null;
        }

        // ---------------------------------------------------------
        // AVAILABLE SYSTEMS
        // ---------------------------------------------------------
        public static IEnumerable<string> GetAvailableSystems()
        {
            if (!Directory.Exists(_romRoot))
                return Enumerable.Empty<string>();

            var folders = Directory.GetDirectories(_romRoot)
                                   .Select(Path.GetFileName)
                                   .Where(f => !string.IsNullOrWhiteSpace(f))
                                   .ToList();

            List<string> validSystems = new();

            foreach (var folder in folders)
            {
                var platform = GetPlatformByFolder(folder);

                if (platform == null)
                {
                    Debug.WriteLine($"[Metadata] SKIP: '{folder}' ignored (no platform definition)");
                    continue;
                }

                if (HasGames(folder))
                {
                    validSystems.Add(folder);
                }
                else
                {
                    Debug.WriteLine($"[Metadata] SKIP: '{folder}' has no games");
                }
            }

            if (validSystems.Any())
                return validSystems;

            // Dev fallback: show all folders
            return folders;
        }

        // ---------------------------------------------------------
        // GAME PRESENCE CHECK
        // ---------------------------------------------------------
        private static bool HasGames(string systemFolderName)
        {
            var platform = GetPlatformByFolder(systemFolderName);
            if (platform != null)
                return GetRomFiles(platform).Any();

            // Fallback: raw folder scan with no platform definition
            string systemPath = Path.Combine(_romRoot, systemFolderName);
            if (!Directory.Exists(systemPath))
                return false;

            // Very conservative: no extensions filter if no platform
            return Directory.EnumerateFiles(systemPath, "*.*", SearchOption.AllDirectories).Any();
        }

        // ---------------------------------------------------------
        // SYSTEM METADATA
        // ---------------------------------------------------------
        public static SystemMetadata GetSystemMetadata(string systemId)
        {
            var platform = GetPlatformByFolder(systemId);

            // ROM files (grouped, multi-disc aware)
            var romFiles = platform != null
                ? GetRomFiles(platform).ToList()
                : GetRomFilesFallback(systemId).ToList();

            // Future: gamelist.xml for per-game metadata (not used for count)
            var gamesFromXml = GetGamesFromGamelist(systemId).ToList();

            return new SystemMetadata
            {
                DebugInfo =
                    $"ID: {systemId}\n" +
                    $"Normalized: {Normalize(systemId)}\n" +
                    $"Matched Platform: {(platform?.Id ?? "NONE")}\n" +
                    $"FolderNames: {string.Join(", ", platform?.FolderNames ?? new List<string>())}\n" +
                    $"Extensions: {string.Join(", ", platform?.Extensions ?? new List<string>())}\n" +
                    $"ROM Files (grouped): {romFiles.Count}\n" +
                    $"XML Entries: {gamesFromXml.Count}",

                Id = platform?.Id ?? systemId,

                DisplayName = platform?.Name
                    ?? platform?.ShortName
                    ?? AutoFormatName(systemId),

                Category = platform?.Category != null
                    ? char.ToUpper(platform.Category[0]) + platform.Category.Substring(1)
                    : "Unknown",

                Description = platform?.Category ?? "Unknown system",

                GameCount = romFiles.Count
            };
        }

        private static string AutoFormatName(string raw)
        {
            var parts = Regex.Split(raw, @"(?<=\D)(?=\d)|(?<=\d)(?=\D)");
            return string.Join(" ", parts.Select(p =>
                char.ToUpper(p[0]) + p.Substring(1)));
        }

        // ---------------------------------------------------------
        // ROM SCANNING (PLATFORM-AWARE, RECURSIVE, GROUPED)
        // ---------------------------------------------------------
        private static IEnumerable<string> GetRomFiles(PlatformDefinition platform)
        {
            if (string.IsNullOrWhiteSpace(_romRoot))
                return Enumerable.Empty<string>();

            // Canonical ROM folder: first folderName, fallback to platform.Id
            string folder = platform.FolderNames.FirstOrDefault() ?? platform.Id;
            string systemPath = Path.Combine(_romRoot, folder);

            if (!Directory.Exists(systemPath))
            {
                Debug.WriteLine($"[Metadata] ROM PATH MISSING: {systemPath}");
                return Enumerable.Empty<string>();
            }

            var exts = (platform.Extensions ?? new List<string>())
                .Select(e => e.StartsWith(".") ? e.ToLowerInvariant() : "." + e.ToLowerInvariant())
                .Distinct()
                .ToList();

            if (!exts.Any())
            {
                Debug.WriteLine($"[Metadata] NO EXTENSIONS for platform '{platform.Id}', path: {systemPath}");
                return Enumerable.Empty<string>();
            }

            // Recursive scan
            var allFiles = Directory.EnumerateFiles(systemPath, "*.*", SearchOption.AllDirectories)
                                    .Where(f =>
                                    {
                                        string ext = Path.GetExtension(f).ToLowerInvariant();
                                        return exts.Contains(ext);
                                    })
                                    .ToList();

            if (!allFiles.Any())
                return Enumerable.Empty<string>();

            // Multi-disc grouping:
            // - Prefer .m3u / .cue as master entries
            // - If present, ignore underlying bin/iso/etc for counting
            var masterExts = new HashSet<string>(new[] { ".m3u", ".cue" });
            var masterFiles = allFiles
                .Where(f => masterExts.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            if (masterFiles.Any())
            {
                // If master files exist, we count only them as "games"
                return masterFiles;
            }

            // Fallback: no .m3u/.cue present
            // For now, count each ROM file as a game (simple mode)
            return allFiles;
        }

        // Fallback when no platform definition exists
        private static IEnumerable<string> GetRomFilesFallback(string systemFolderName)
        {
            string systemPath = Path.Combine(_romRoot, systemFolderName);
            if (!Directory.Exists(systemPath))
                return Enumerable.Empty<string>();

            return Directory.EnumerateFiles(systemPath, "*.*", SearchOption.AllDirectories);
        }

        // ---------------------------------------------------------
        // GAMELIST.XML PARSING (FUTURE UI METADATA)
        // ---------------------------------------------------------
        private static IEnumerable<GameMetadata> GetGamesFromGamelist(string systemId)
        {
            string systemPath = Path.Combine(_romRoot, systemId);
            if (!Directory.Exists(systemPath))
                return Enumerable.Empty<GameMetadata>();

            string xmlPath = Path.Combine(systemPath, "gamelist.xml");
            if (!File.Exists(xmlPath))
                return Enumerable.Empty<GameMetadata>();

            try
            {
                var doc = XDocument.Load(xmlPath);
                return doc.Root?
                    .Elements("game")
                    .Select(x => new GameMetadata
                    {
                        Name = x.Element("name")?.Value ?? "Unknown",
                        Path = x.Element("path")?.Value ?? "",
                        Description = x.Element("desc")?.Value ?? "",
                        Image = x.Element("image")?.Value ?? "",
                        Video = x.Element("video")?.Value ?? "",
                        ReleaseDate = x.Element("releasedate")?.Value ?? "",
                        Developer = x.Element("developer")?.Value ?? "",
                        Publisher = x.Element("publisher")?.Value ?? "",
                        Genre = x.Element("genre")?.Value ?? "",
                        Players = x.Element("players")?.Value ?? ""
                    })
                    .ToList()
                    ?? Enumerable.Empty<GameMetadata>();
            }
            catch
            {
                return Enumerable.Empty<GameMetadata>();
            }
        }
    }
}