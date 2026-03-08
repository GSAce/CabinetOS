namespace CabinetOS.Core.Platforms.Detection;

public interface IPlatformDetector
{
    Task<IReadOnlyList<DetectedPlatform>> DetectAsync(
        string romRoot,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default);
}