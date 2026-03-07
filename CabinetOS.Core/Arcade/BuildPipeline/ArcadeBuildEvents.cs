namespace CabinetOS.Core.Arcade.BuildPipeline
{
    public class ArcadeBuildEvents
    {
        public Action<string>? OnStatus { get; set; }
        public Action<int, int, string>? OnGameProgress { get; set; }
        public Action<string, string>? OnRomProgress { get; set; }
        public Action<string>? OnChdProgress { get; set; }
        public Action<string>? OnLog { get; set; }
        public Action? OnCompleted { get; set; }
        public Action<Exception>? OnError { get; set; }
    }
}