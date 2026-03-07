using System.Collections.Generic;
using System.Linq;
using CabinetOS.Core.Arcade.Models;

namespace CabinetOS.Core.Arcade.Filtering
{
    public static class ArcadeGameFilter
    {
        public static List<DatGameInfo> ApplyFilters(
            IEnumerable<DatGameInfo> games,
            ArcadeFilterOptions options)
        {
            var result = games;

            // ------------------------------------------------------------
            // 1. Exclude BIOS entries
            // ------------------------------------------------------------
            if (options.ExcludeBios)
            {
                result = result.Where(g => !g.IsBios).ToList();
            }

            // ------------------------------------------------------------
            // 2. Exclude mechanical games
            // ------------------------------------------------------------
            if (options.ExcludeMechanical)
            {
                result = result.Where(g =>
                    !g.Categories.Any(c =>
                        c.Equals("mechanical", System.StringComparison.OrdinalIgnoreCase)
                    )).ToList();
            }

            // ------------------------------------------------------------
            // 3. Exclude mature games
            // ------------------------------------------------------------
            if (options.ExcludeMature)
            {
                result = result.Where(g =>
                    !g.Categories.Any(c =>
                        c.Equals("mature", System.StringComparison.OrdinalIgnoreCase)
                    )).ToList();
            }

            // ------------------------------------------------------------
            // 4. Exclude clones
            // ------------------------------------------------------------
            if (options.ExcludeClones)
            {
                result = result.Where(g => string.IsNullOrEmpty(g.CloneOf)).ToList();
            }

            // ------------------------------------------------------------
            // 5. Working-only filter
            // ------------------------------------------------------------
            if (options.WorkingOnly)
            {
                result = result.Where(g => g.IsWorking).ToList();
            }

            // ------------------------------------------------------------
            // 6. Region preference filter
            // ------------------------------------------------------------
            if (options.AllowedRegions.Count > 0)
            {
                result = result.Where(g =>
                    options.AllowedRegions.Any(region =>
                        g.Name.Contains(region, System.StringComparison.OrdinalIgnoreCase)
                    )).ToList();
            }

            // ------------------------------------------------------------
            // 7. Excluded categories
            // ------------------------------------------------------------
            if (options.ExcludedCategories.Count > 0)
            {
                result = result.Where(g =>
                    !g.Categories.Any(c =>
                        options.ExcludedCategories.Contains(
                            c, System.StringComparer.OrdinalIgnoreCase
                        )
                    )).ToList();
            }

            return result.ToList();
        }
    }
}