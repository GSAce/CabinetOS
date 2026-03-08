using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class AppearanceSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Appearance";

        private readonly AppearanceSettingsView _view = new AppearanceSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public AppearanceSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public AppearanceSettings Appearance
        {
            get => _settingsService.Current.Appearance;
            set
            {
                _settingsService.Current.Appearance = value;
                OnPropertyChanged();
            }
        }

        public void Load() => OnPropertyChanged(nameof(Appearance));

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.Appearance = new AppearanceSettings();
            Load();
        }
    }
}