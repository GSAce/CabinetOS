using System.Collections.Generic;
using System.IO;
using System.Linq;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class M3UGenerator
    {
        public void Generate(string outputFolder, string gameName, List<RomInfo> discs)
        {
            var m3uPath = Path.Combine(outputFolder, $"{gameName}.m3u");

            var lines = discs
                .OrderBy(d => d.DiscId)
                .Select(d => d.FileName)
                .ToList();

            File.WriteAllLines(m3uPath, lines);
        }
    }
}