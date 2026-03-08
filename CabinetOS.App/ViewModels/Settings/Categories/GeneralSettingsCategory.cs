using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class GeneralSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "General";

        private readonly GeneralSettingsView _view = new GeneralSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public GeneralSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public GeneralSettings General
        {
            get => _settingsService.Current.General;
            set
            {
                _settingsService.Current.General = value;
                OnPropertyChanged();
            }
        }

        public void Load() => OnPropertyChanged(nameof(General));

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.General = new GeneralSettings();
            Load();
        }
    }
}