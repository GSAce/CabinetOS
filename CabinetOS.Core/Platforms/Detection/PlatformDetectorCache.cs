namespace CabinetOS.Core.Platforms.Detection;

public class PlatformDetectionCache
{
    private readonly Dictionary<string, (DateTime Timestamp, IReadOnlyList<DetectedPlatform> Result)> _cache = new();
    private readonly TimeSpan _ttl;

    public PlatformDetectionCache(TimeSpan? ttl = null)
    {
        _ttl = ttl ?? TimeSpan.FromSeconds(10);
    }

    public bool TryGet(string romRoot, out IReadOnlyList<DetectedPlatform> result)
    {
        if (_cache.TryGetValue(romRoot, out var entry))
        {
            if (DateTime.UtcNow - entry.Timestamp <= _ttl)
            {
                result = entry.Result;
                return true;
            }
            _cache.Remove(romRoot);
        }

        result = Array.Empty<DetectedPlatform>();
        return false;
    }

    public void Set(string romRoot, IReadOnlyList<DetectedPlatform> result)
    {
        _cache[romRoot] = (DateTime.UtcNow, result);
    }
}