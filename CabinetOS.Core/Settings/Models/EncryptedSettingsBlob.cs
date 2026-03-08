namespace CabinetOS.Core.Settings.Models
{
    public class EncryptedSettingsBlob
    {
        // IGDB
        public string IgdbClientId { get; set; } = "";
        public string IgdbClientSecret { get; set; } = "";

        // ScreenScraper developer credentials
        public string ScreenScraperDevId { get; set; } = "";
        public string ScreenScraperDevPassword { get; set; } = "";

        // ScreenScraper user login
        public string ScreenScraperUserName { get; set; } = "";
        public string ScreenScraperUserPassword { get; set; } = "";

        // ScreenScraper API key
        public string ScreenScraperApiKey { get; set; } = "";

        // TheGamesDb
        public string TheGamesDbApiKey { get; set; } = "";
    }
}