using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class AdvancedSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Advanced";

        private readonly AdvancedSettingsView _view = new AdvancedSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public AdvancedSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public AdvancedSettings Advanced
        {
            get => _settingsService.Current.Advanced;
            set
            {
                _settingsService.Current.Advanced = value;
                OnPropertyChanged();
            }
        }

        public void Load() => OnPropertyChanged(nameof(Advanced));

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.Advanced = new AdvancedSettings();
            Load();
        }
    }
}