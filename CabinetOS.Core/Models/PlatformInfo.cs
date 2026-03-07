namespace CabinetOS.Core.Models
{
    public class PlatformInfo
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public List<string> Extensions { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public string ScraperId { get; set; } = string.Empty;
    }
}