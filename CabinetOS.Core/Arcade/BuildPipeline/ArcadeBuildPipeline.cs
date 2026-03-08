using System;
using CabinetOS.Core.Arcade.Filtering;
using CabinetOS.Core.Arcade.Orchestration;

namespace CabinetOS.Core.Arcade.BuildPipeline
{
    public static class ArcadeBuildPipeline
    {
        public static void StartBuild(
            string sourceFolder,
            string outputFolder,
            string? familyOverride,
            ArcadeFilterOptions? filterOptions,
            ArcadeBuildEvents events)
        {
            try
            {
                events.OnStatus?.Invoke("Starting build...");

                var summary = ArcadeBuildOrchestrator.BuildArcadeSet(
                    sourceFolder,
                    outputFolder,
                    familyOverride,
                    filterOptions,
                    dryRun: false,
                    events.OnStatus,
                    events.OnRomProgress,
                    events.OnChdProgress
                );

                events.OnStatus?.Invoke("Build complete.");
                events.OnCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                events.OnError?.Invoke(ex);
            }
        }
    }
}