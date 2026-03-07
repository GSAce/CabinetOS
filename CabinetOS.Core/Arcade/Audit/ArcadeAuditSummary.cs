using System.Collections.Generic;

namespace CabinetOS.Core.Arcade.Audit
{
    /// <summary>
    /// High-level summary of an arcade audit for a single DAT family.
    /// </summary>
    public class ArcadeAuditSummary
    {
        public string Family { get; set; } = "";

        public int TotalGames { get; set; }
        public int GamesFullyPresent { get; set; }
        public int GamesWithMissingRoms { get; set; }
        public int GamesWithMissingChds { get; set; }

        public int TotalRoms { get; set; }
        public int MissingRoms { get; set; }

        public int TotalChds { get; set; }
        public int MissingChds { get; set; }

        /// <summary>
        /// Optional: list of game names that are incomplete.
        /// </summary>
        public List<string> IncompleteGames { get; set; } = new();
    }
}