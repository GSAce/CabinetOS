namespace CabinetOS.Core.Platforms.Detection;

public static class TempPlatformDefaults
{
    public static readonly Dictionary<string, PlatformDefinition> All =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "nes",
                new PlatformDefinition
                {
                    Id = "nes",
                    ShortName = "NES",
                    Name = "Nintendo Entertainment System",
                    Category = "Console",
                    FolderNames = new List<string> { "nes" },
                    Extensions = new List<string> { "nes", "zip" }
                }
            },
            {
                "snes",
                new PlatformDefinition
                {
                    Id = "snes",
                    ShortName = "SNES",
                    Name = "Super Nintendo",
                    Category = "Console",
                    FolderNames = new List<string> { "snes" },
                    Extensions = new List<string> { "sfc", "smc", "zip" }
                }
            },
            {
                "genesis",
                new PlatformDefinition
                {
                    Id = "genesis",
                    ShortName = "GEN",
                    Name = "Sega Genesis",
                    Category = "Console",
                    FolderNames = new List<string> { "genesis", "megadrive" },
                    Extensions = new List<string> { "bin", "gen", "md", "zip" }
                }
            },
            {
                "arcade",
                new PlatformDefinition
                {
                    Id = "arcade",
                    ShortName = "Arcade",
                    Name = "Arcade (MAME)",
                    Category = "Arcade",
                    FolderNames = new List<string> { "arcade", "mame" },
                    Extensions = new List<string> { "zip" }
                }
            }
        };
}