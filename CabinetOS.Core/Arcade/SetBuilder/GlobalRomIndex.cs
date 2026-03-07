using System.Collections.Generic;

namespace CabinetOS.Core.Arcade.SetBuilder
{
    /// <summary>
    /// Global index of all ROMs and CHDs found during scanning.
    /// Used by SetBuilder for fast lookup, salvage mode, and future deep-audit support.
    /// </summary>
    public class GlobalRomIndex
    {
        /// <summary>
        /// Maps ROM filename → archive path.
        /// Example: "sf2.zip" → "D:/ArcadeSource/sf2.zip"
        /// </summary>
        public Dictionary<string, string> ByName { get; set; } = new();

        /// <summary>
        /// Maps CHD filename → full CHD path.
        /// Example: "gdrom.chd" → "D:/ArcadeSource/chd/naomi/gdrom.chd"
        /// </summary>
        public Dictionary<string, string> ByChdName { get; set; } = new();

        /// <summary>
        /// Maps SHA1 → archive path or CHD path.
        /// Useful for deep audit, salvage mode, and future hashing.
        /// </summary>
        public Dictionary<string, string> BySha1 { get; set; } = new();

        /// <summary>
        /// Maps CRC → archive path.
        /// Useful for ROM-level salvage and validation.
        /// </summary>
        public Dictionary<string, string> ByCrc { get; set; } = new();

        /// <summary>
        /// Maps MD5 → archive path or CHD path.
        /// Useful for future hashing and deep audit integration.
        /// </summary>
        public Dictionary<string, string> ByMd5 { get; set; } = new();
    }
}