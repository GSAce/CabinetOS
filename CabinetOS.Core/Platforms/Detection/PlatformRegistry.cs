using System.Text.Json;

namespace CabinetOS.Core.Platforms.Detection;

public class PlatformRegistry
{
    public IReadOnlyDictionary<string, PlatformDefinition> Platforms => _platforms;
    private readonly Dictionary<string, PlatformDefinition> _platforms;

    public PlatformRegistry(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        _platforms = JsonSerializer.Deserialize<Dictionary<string, PlatformDefinition>>(json)
                     ?? new Dictionary<string, PlatformDefinition>(StringComparer.OrdinalIgnoreCase);
    }
}