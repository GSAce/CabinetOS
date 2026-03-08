using System.Collections.Generic;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class RomScanResult
    {
        public Dictionary<string, List<RomInfo>> Platforms { get; set; } = new();
        public int TotalFiles { get; set; }
        public int TotalPlatforms => Platforms.Count;
    }
}