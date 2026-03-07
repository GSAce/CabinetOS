using System.Text.Json;
using System.Text.Json.Serialization;

namespace CabinetOS.Core.Arcade.SetBuilder
{
    public static class BuildSummaryJson
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string ToJson(BuildSummary summary)
        {
            var dto = new
            {
                summary.TotalGames,
                summary.BuiltGames,

                // ROMs
                summary.MissingRoms,
                summary.SalvagedRoms,
                summary.MissingRomList,

                // CHDs
                summary.TotalChds,
                summary.AddedChds,
                summary.SalvagedChds,
                summary.MissingChds,
                summary.MissingChdList,

                // Build metadata
                summary.ElapsedTime,
                DryRun = true, // because this is only called in dry run mode

                // Optional: include log lines
                Log = summary.Log?.Entries
            };

            return JsonSerializer.Serialize(dto, Options);
        }
    }
}