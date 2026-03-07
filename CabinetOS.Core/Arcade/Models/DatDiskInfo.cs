using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.Models
{

    public class DatDiskInfo
    {
        public string Name { get; set; } = string.Empty;  // CHD name
        public string? MD5 { get; set; }
        public string? SHA1 { get; set; }
        public string? Region { get; set; }
    }
}
