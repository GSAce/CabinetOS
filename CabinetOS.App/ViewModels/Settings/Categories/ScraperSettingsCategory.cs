using CabinetOS.Core.Settings;
using CabinetOS.App.Views.Settings.Categories;

using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class ScraperSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Scraper";

        private readonly ScraperSettingsView _view = new ScraperSettingsView();
        public object View => _view;

        private ScraperSettings _settings = new();
        public ScraperSettings Settings
        {
            get => _settings;
            set => SetField(ref _settings, value);
        }

        // Encryption key (you can later move this to a secure provider)
        private const string EncryptionKey = "CabinetOS_Scraper_Key";

        public ScraperSettingsCategory()
        {
            Load();
        }

        public void Load()
        {
            Settings = SettingsService.Load<ScraperSettings>(
                SettingsPaths.ScraperSettingsFile,
                EncryptionKey
            );
        }

        public void Save()
        {
            SettingsService.Save(
                SettingsPaths.ScraperSettingsFile,
                Settings,
                EncryptionKey
            );
        }

        public void ResetToDefaults()
        {
            Settings = new ScraperSettings();
        }
    }
}