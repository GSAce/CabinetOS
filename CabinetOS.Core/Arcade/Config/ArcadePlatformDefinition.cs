namespace CabinetOS.Core.Arcade.Config
{
    public class ArcadePlatformDefinition
    {
        public string Name { get; set; } = "Arcade";
        public string ShortName { get; set; } = "arcade";
        public string Category { get; set; } = "Arcade";
        public string Emulator { get; set; } = "mame";

        public string MergeMode { get; set; } = "split"; // "split", "nonmerged", "merged"

        public string DefaultFamily { get; set; } = "";  // e.g. "mame285"

        public List<string> Families { get; set; } = new(); // allowed DAT families

        public List<string> BiosFamilies { get; set; } = new(); // e.g. neogeo, naomi
        public List<string> RegionPreference { get; set; } = new(); // ["USA","World","Japan"]
        public string? SourceFolder { get; set; }
        public string? OutputFolder { get; set; }
    }
}