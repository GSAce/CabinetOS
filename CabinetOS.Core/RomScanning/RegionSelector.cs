using System;
using System.Collections.Generic;
using System.Linq;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class RegionSelector
    {
        private readonly List<string> _preference;

        public RegionSelector(IEnumerable<string> regionPreference)
        {
            _preference = regionPreference?
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList()
                ?? new List<string>();
        }

        public List<RomInfo> SelectBestRegion(List<RomInfo> roms)
        {
            if (_preference.Count == 0)
                return roms;

            // Group by region
            var byRegion = roms
                .GroupBy(r => r.Region ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

            // Try preferred regions in order
            foreach (var region in _preference)
            {
                if (byRegion.TryGetValue(region, out var list) && list.Count > 0)
                    return list;
            }

            // Fallback: any region
            return roms;
        }
    }
}