using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class ScraperSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Scraper";

        private readonly ScraperSettingsView _view = new ScraperSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public ScraperSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public ScraperSettings Scraper
        {
            get => _settingsService.Current.Scraper;
            set
            {
                _settingsService.Current.Scraper = value;
                OnPropertyChanged();
            }
        }

        public ScraperNetworkSettings Network
        {
            get => _settingsService.Current.ScraperNetwork;
            set
            {
                _settingsService.Current.ScraperNetwork = value;
                OnPropertyChanged();
            }
        }

        public IgdbConfig Igdb
        {
            get => _settingsService.Current.Igdb;
            set
            {
                _settingsService.Current.Igdb = value;
                OnPropertyChanged();
            }
        }

        public ScreenScraperConfig ScreenScraper
        {
            get => _settingsService.Current.ScreenScraper;
            set
            {
                _settingsService.Current.ScreenScraper = value;
                OnPropertyChanged();
            }
        }

        public TheGamesDbConfig TheGamesDb
        {
            get => _settingsService.Current.TheGamesDb;
            set
            {
                _settingsService.Current.TheGamesDb = value;
                OnPropertyChanged();
            }
        }

        public void Load()
        {
            OnPropertyChanged(nameof(Scraper));
            OnPropertyChanged(nameof(Network));
            OnPropertyChanged(nameof(Igdb));
            OnPropertyChanged(nameof(ScreenScraper));
            OnPropertyChanged(nameof(TheGamesDb));
        }

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.Scraper = new ScraperSettings();
            _settingsService.Current.ScraperNetwork = new ScraperNetworkSettings();
            _settingsService.Current.Igdb = new IgdbConfig();
            _settingsService.Current.ScreenScraper = new ScreenScraperConfig();
            _settingsService.Current.TheGamesDb = new TheGamesDbConfig();

            Load();
        }
    }
}