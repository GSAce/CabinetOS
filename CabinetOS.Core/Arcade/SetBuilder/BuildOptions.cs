using CabinetOS.Core.Arcade.Models;
using CabinetOS.Core.Arcade.SetBuilder;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CabinetOS.Core.Arcade
{
    public class BuildOptions
    {
        // ------------------------------------------------------------
        // REQUIRED
        // ------------------------------------------------------------
        public string SourceFolder { get; set; } = "";
        public string OutputFolder { get; set; } = "";
        public List<DatGameInfo> Games { get; set; } = new();
        public SetBuildMode Mode { get; set; } = SetBuildMode.Merged;
        public bool DryRun { get; set; }

        // ------------------------------------------------------------
        // OPTIONAL
        // ------------------------------------------------------------
        public GlobalRomIndex? SalvageIndex { get; set; }
        public bool Parallel { get; set; } = false;

        // ------------------------------------------------------------
        // PROGRESS CALLBACKS
        // ------------------------------------------------------------
        public Action<int, int, string>? GameProgress { get; set; }
        public Action<string, string>? RomProgress { get; set; }
        public Action<string>? SalvageProgress { get; set; }
        public Action<string, string>? ChdProgress { get; set; }

        // ------------------------------------------------------------
        // LOGGING
        // ------------------------------------------------------------
        public BuildLog? Log { get; set; }

        // ------------------------------------------------------------
        // CANCELLATION
        // ------------------------------------------------------------
        public CancellationToken? Cancellation { get; set; }
    }
}