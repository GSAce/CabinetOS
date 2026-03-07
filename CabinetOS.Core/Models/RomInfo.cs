namespace CabinetOS.Core.Models
{
    public class RomInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string? MD5 { get; set; }
        public string? SHA1 { get; set; }
        public PlatformInfo? Platform { get; set; }

        // Future expansion:
        // public string? Region { get; set; }
        // public string? DiscId { get; set; }
        // public string? Hash { get; set; }
        // public bool IsMultiDisc { get; set; }
    }
}