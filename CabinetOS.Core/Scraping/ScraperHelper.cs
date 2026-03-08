using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.Core.Scraping
{
    public class ScraperHelper
    {
        private readonly SettingsService _settings;

        public ScraperHelper(SettingsService settings)
        {
            _settings = settings;
        }

        // ---------------------------
        // IGDB
        // ---------------------------
        public string IgdbClientId =>
            _settings.Current.Igdb.ClientId;

        public string IgdbClientSecret =>
            _settings.Current.Igdb.ClientSecretPlain; // decrypted at runtime


        // ---------------------------
        // ScreenScraper (Developer)
        // ---------------------------
        public string ScreenScraperDevId =>
            _settings.Current.ScreenScraper.DevId;

        public string ScreenScraperDevPassword =>
            _settings.Current.ScreenScraper.DevPasswordPlain;


        // ---------------------------
        // ScreenScraper (User Login)
        // ---------------------------
        public string ScreenScraperUserName =>
            _settings.Current.ScreenScraper.UserName;

        public string ScreenScraperUserPassword =>
            _settings.Current.ScreenScraper.UserPasswordPlain;


        // ---------------------------
        // ScreenScraper (API Key)
        // ---------------------------
        public string ScreenScraperApiKey =>
            _settings.Current.ScreenScraper.ApiKeyPlain;


        // ---------------------------
        // TheGamesDb
        // ---------------------------
        public string TheGamesDbApiKey =>
            _settings.Current.TheGamesDb.ApiKey;


        // ---------------------------
        // Network behavior
        // ---------------------------
        public int TimeoutSeconds =>
            _settings.Current.ScraperNetwork.TimeoutSeconds;

        public int RetryCount =>
            _settings.Current.ScraperNetwork.RetryCount;

        public bool UseProxy =>
            _settings.Current.ScraperNetwork.UseProxy;

        public string ProxyAddress =>
            _settings.Current.ScraperNetwork.ProxyAddress;

        public int ProxyPort =>
            _settings.Current.ScraperNetwork.ProxyPort;
    }
}