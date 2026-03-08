using System.Collections.Concurrent;

namespace CabinetOS.Core.Platforms.Detection;

public class PlatformDetector : IPlatformDetector
{
    private readonly PlatformRegistry _registry;
    private readonly PlatformDetectionCache _cache;

    public PlatformDetector(PlatformRegistry registry, PlatformDetectionCache? cache = null)
    {
        _registry = registry;
        _cache = cache ?? new PlatformDetectionCache();
    }

    public async Task<IReadOnlyList<DetectedPlatform>> DetectAsync(
        string romRoot,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (_cache.TryGet(romRoot, out var cached))
            return cached;

        var platforms = _registry.Platforms.Values.ToList();
        if (platforms.Count == 0)
            return Array.Empty<DetectedPlatform>();

        var results = new ConcurrentBag<DetectedPlatform>();
        var total = platforms.Count;
        var processed = 0;

        await Task.Run(() =>
        {
            Parallel.ForEach(platforms, new ParallelOptions
            {
                CancellationToken = cancellationToken
            }, platform =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var detected = DetectPlatform(romRoot, platform);
                if (detected != null)
                    results.Add(detected);

                var done = Interlocked.Increment(ref processed);
                progress?.Report((double)done / total);
            });
        }, cancellationToken);

        var final = results
            .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList()
            .AsReadOnly();

        _cache.Set(romRoot, final);
        return final;
    }

    private DetectedPlatform? DetectPlatform(string romRoot, PlatformDefinition platform)
    {
        foreach (var folderName in platform.FolderNames)
        {
            var folderPath = Path.Combine(romRoot, folderName);
            if (!Directory.Exists(folderPath))
                continue;

            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories);
            var count = 0;

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file).ToLowerInvariant();
                if (platform.Extensions.Contains(ext))
                    count++;
            }

            if (count > 0)
            {
                return new DetectedPlatform
                {
                    Id = platform.Id,
                    Name = platform.Name,
                    Category = platform.Category,
                    RootFolder = folderPath,
                    FileCount = count
                };
            }
        }

        return null;
    }
}