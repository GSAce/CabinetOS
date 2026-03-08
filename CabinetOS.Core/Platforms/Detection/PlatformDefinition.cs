namespace CabinetOS.Core.Platforms.Detection;

public class PlatformDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Shortname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> FolderNames { get; set; } = new();
    public List<string> Extensions { get; set; } = new();
}