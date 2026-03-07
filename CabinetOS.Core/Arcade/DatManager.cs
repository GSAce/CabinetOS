using CabinetOS.Core.Arcade.DatDiscovery;
using CabinetOS.Core.Arcade.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CabinetOS.Core.Arcade
{
    public static class DatManager
    {
        private static readonly Dictionary<string, List<DatGameInfo>> _dats =
            new(StringComparer.OrdinalIgnoreCase);

        private static bool _loaded = false;

        public static void EnsureLoaded()
        {
            if (_loaded)
                return;

            string datFilesRoot = Path.Combine(AppContext.BaseDirectory, "Arcade", "DatFiles");

            if (!Directory.Exists(datFilesRoot))
            {
                _loaded = true;
                return;
            }

            // Load categories first
            CategoryManager.EnsureLoaded();

            // Use the new scanner
            var families = ArcadeDatScanner.Scan(datFilesRoot);

            foreach (var family in families)
            {
                var mergedGames = new List<DatGameInfo>();

                foreach (var datPath in family.DatPaths)
                {
                    var games = DatParserService.LoadDat(datPath);
                    mergedGames.AddRange(games);
                }

                // Merge categories into each game
                foreach (var game in mergedGames)
                    game.Categories = CategoryManager.GetCategories(game.Name);

                _dats[family.Family] = mergedGames;
            }

            _loaded = true;
        }

        public static List<DatGameInfo>? GetDat(string familyName)
        {
            EnsureLoaded();

            if (_dats.TryGetValue(familyName, out var games))
                return games;

            return null;
        }

        public static IEnumerable<string> GetAvailableSystems()
        {
            EnsureLoaded();
            return _dats.Keys;
        }
    }
}