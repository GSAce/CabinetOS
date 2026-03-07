using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.SetBuilder
{
    // ============================================================
    // BUILD SUMMARY
    // ============================================================
    public class BuildSummary
    {
        public int TotalGames { get; set; }
        public int BuiltGames { get; set; }
        public int MissingRoms { get; set; }
        public int SalvagedRoms { get; set; }
        public BuildLog? Log { get; set; }
        public int TotalChds { get; set; }
        public int AddedChds { get; set; }
        public int SalvagedChds { get; set; }
        public int MissingChds { get; set; }

        public List<string> MissingRomList { get; } = new();
        public List<string> MissingChdList { get; } = new();

        public string? ElapsedTime { get; set; }


    }
}
