using CabinetOS.Core.Settings.Models;

namespace CabinetOS.Core.Settings.Models
{
    public class CabinetSettings
    {
        // General application behavior
        public GeneralSettings General { get; set; } = new();

        // All filesystem paths used by CabinetOS
        public PathsSettings Paths { get; set; } = new();

        // Network, proxy, timeouts, online/offline behavior
        public NetworkSettings Network { get; set; } = new();              // WiFi, DirectLink, etc.
        public ScraperNetworkSettings ScraperNetwork { get; set; } = new(); // HTTP/API behavior

        // UI theme, colors, scaling, animations
        public AppearanceSettings Appearance { get; set; } = new();

        // Scraper toggles and global scraper behavior
        public ScraperSettings Scraper { get; set; } = new();

        // IGDB API configuration (encrypted secret)
        public IgdbConfig Igdb { get; set; } = new();

        // ScreenScraper configuration (dev login, user login, API key)
        public ScreenScraperConfig ScreenScraper { get; set; } = new();

        // TheGamesDb API configuration
        public TheGamesDbConfig TheGamesDb { get; set; } = new();

        // Arcade-specific settings (DAT family, CHD paths, etc.)
        public ArcadeSettings Arcade { get; set; } = new();

        // Developer/debug/advanced toggles
        public AdvancedSettings Advanced { get; set; } = new();
    }
}