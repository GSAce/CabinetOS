using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.Models
{
    public class RomDependencyResult
    {
        public List<DatRomInfo> RequiredRoms { get; set; } = new();
        public List<DatDiskInfo> RequiredDisks { get; set; } = new();


    }


}
