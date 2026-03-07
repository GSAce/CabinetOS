namespace CabinetOS.Core.Arcade.Filtering
{
    public class ArcadeFilterOptions
    {
        public bool ExcludeClones { get; set; } = false;
        public bool ExcludeMechanical { get; set; } = true;
        public bool ExcludeMature { get; set; } = false;
        public bool ExcludeBios { get; set; } = true;
        public bool WorkingOnly { get; set; } = false;

        public List<string> AllowedRegions { get; set; } = new();
        public List<string> ExcludedCategories { get; set; } = new();
    }
}