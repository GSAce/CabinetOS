using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Config.Settings
{
    public class ArcadeSettings
    {
        public string SourceFolder { get; set; } = "";
        public string OutputFolder { get; set; } = "";
        public string ChdFolder { get; set; } = "";
        public string TempFolder { get; set; } = "";
        public string DefaultFamily { get; set; } = "";
    }
}