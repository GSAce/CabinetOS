namespace CabinetOS.Core.Platforms.Detection;

public class DetectedPlatform
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string RootFolder { get; set; } = string.Empty;
    public int FileCount { get; set; }
}