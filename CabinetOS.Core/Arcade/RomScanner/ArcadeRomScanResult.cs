using System.Collections.Generic;
using CabinetOS.Core.Arcade.SetBuilder;

namespace CabinetOS.Core.Arcade.RomScanner
{
    public class ArcadeRomScanResult
    {
        public int TotalArchives { get; set; }
        public int TotalRoms { get; set; }
        public int TotalChds { get; set; }

        public List<string> MissingRoms { get; set; } = new();
        public List<string> MissingChds { get; set; } = new();

        public GlobalRomIndex Index { get; set; } = new GlobalRomIndex();
        public bool SalvageModeEnabled { get; set; }
    }
}