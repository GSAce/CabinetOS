using System;

namespace CabinetOS.Core.Arcade.RomScanner.Hashing
{
    public class HashCacheEntry
    {
        public string Path { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModifiedUtc { get; set; }

        public string? SHA1 { get; set; }
        public string? MD5 { get; set; }
        public string? CRC { get; set; }
    }
}