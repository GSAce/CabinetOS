using System;
using System.Collections.Generic;
using System.Text;

namespace CabinetOS.Core.Arcade.Models
{
    public class DatRomInfo
    {
        public string Name { get; set; } = string.Empty; // rom file name
        public long Size { get; set; }                   // size in bytes
        public string? CRC { get; set; }
        public string? MD5 { get; set; }
        public string? SHA1 { get; set; }
        public bool IsBios { get; set; }
        public bool IsDevice { get; set; }
        public bool IsSample { get; set; }
    }
}
