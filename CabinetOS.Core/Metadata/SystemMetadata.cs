namespace CabinetOS.Core.Metadata
{
    public class SystemMetadata
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int GameCount { get; set; }
        public string DebugInfo { get; set; } = string.Empty;
    }
}