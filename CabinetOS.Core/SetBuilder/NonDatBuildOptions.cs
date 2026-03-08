using System.Collections.Generic;

namespace CabinetOS.Core.SetBuilder
{
    public class NonDatBuildOptions
    {
        /// <summary>
        /// Root folder where source ROMs live (same root used by RomManager / NonDatScanner).
        /// </summary>
        public string RomRoot { get; set; } = string.Empty;

        /// <summary>
        /// Output folder where the built set will be created.
        /// </summary>
        public string OutputRoot { get; set; } = string.Empty;

        /// <summary>
        /// Platform IDs to include (matching PlatformDefinition.Id).
        /// If empty, all detected platforms are included.
        /// </summary>
        public List<string> IncludedPlatforms { get; set; } = new();

        /// <summary>
        /// Whether to normalize ROM names before building (via RomManager).
        /// </summary>
        public bool NormalizeNames { get; set; }

        /// <summary>
        /// Whether to backup original names before normalization (via RomManager).
        /// </summary>
        public bool BackupNames { get; set; }

        /// <summary>
        /// Whether to include BIOS files in the output.
        /// </summary>
        public bool IncludeBios { get; set; }

        /// <summary>
        /// Whether to include CHD files in the output.
        /// </summary>
        public bool IncludeChd { get; set; } = true;

        /// <summary>
        /// Region preference order, e.g. ["USA", "Europe", "Japan"].
        /// </summary>
        public List<string> RegionPreference { get; set; } = new();

        /// <summary>
        /// Whether to group multi-disc games into subfolders.
        /// </summary>
        public bool GroupMultiDisc { get; set; } = true;
    }
}