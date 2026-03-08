using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class DiscGroupDetector
    {
        public Dictionary<string, List<RomInfo>> GroupByGame(List<RomInfo> roms)
        {
            var dict = new Dictionary<string, List<RomInfo>>(StringComparer.OrdinalIgnoreCase);

            foreach (var rom in roms)
            {
                var key = BuildGameKey(rom);

                if (!dict.TryGetValue(key, out var list))
                {
                    list = new List<RomInfo>();
                    dict[key] = list;
                }

                list.Add(rom);
            }

            return dict;
        }

        private string BuildGameKey(RomInfo rom)
        {
            var name = Path.GetFileNameWithoutExtension(rom.FileName);
            var lower = name.ToLowerInvariant();

            // Strip region and disc markers
            var markers = new[]
            {
                "(usa)", "(u)",
                "(europe)", "(e)",
                "(japan)", "(j)",
                "(world)",
                "(disc 1)", "(disc 2)", "(disc 3)", "(disc 4)",
                "(cd1)", "(cd2)", "(cd3)", "(cd4)",
                "(disk 1)", "(disk 2)", "(disk 3)", "(disk 4)"
            };

            foreach (var marker in markers)
            {
                var idx = lower.IndexOf(marker, StringComparison.Ordinal);
                if (idx >= 0)
                {
                    name = name.Remove(idx, marker.Length);
                    lower = name.ToLowerInvariant();
                }
            }

            return name.Trim();
        }
    }
}