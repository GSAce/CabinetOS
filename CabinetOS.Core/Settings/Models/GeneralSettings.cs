namespace CabinetOS.Core.Settings.Models
{
    public class GeneralSettings
    {
        public string Language { get; set; } = "en";
        public bool AutoCheckUpdates { get; set; } = true;
        public bool ConfirmBeforeExit { get; set; } = true;

        public enum HashScanMode
        {
            Fast,
            Auto,
            Deep
        }

        public HashScanMode HashMode { get; set; } = HashScanMode.Auto;
    }
}