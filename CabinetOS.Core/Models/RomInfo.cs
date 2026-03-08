namespace CabinetOS.Core.Models
{
    public class RomInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;

        // Hashes (optional but supported)
        public string? MD5 { get; set; }
        public string? SHA1 { get; set; }

        // Platform association
        public PlatformInfo? Platform { get; set; }

        // Normalization support
        public string? OriginalName { get; set; }
        public bool IsNormalized { get; set; }

        // Classification
        public bool IsBios { get; set; }
        public bool IsChd { get; set; }

        // Future expansion
        public string? Region { get; set; }
        public string? DiscId { get; set; }
        public bool IsMultiDisc { get; set; }
    }
}