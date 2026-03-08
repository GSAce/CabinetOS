using System.Collections.Generic;

namespace CabinetOS.Core.SetBuilder
{
    public class NonDatBuildSummary
    {
        public int TotalPlatforms { get; set; }
        public int TotalGames { get; set; }
        public int TotalRoms { get; set; }
        public int TotalChds { get; set; }
        public long TotalBytes { get; set; }

        public List<string> Warnings { get; } = new();
        public List<string> Errors { get; } = new();
    }
}