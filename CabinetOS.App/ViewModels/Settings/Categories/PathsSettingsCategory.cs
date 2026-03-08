using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class PathsSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Paths";

        private readonly PathsSettingsView _view = new PathsSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public PathsSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public PathsSettings Paths
        {
            get => _settingsService.Current.Paths;
            set
            {
                _settingsService.Current.Paths = value;
                OnPropertyChanged();
            }
        }

        public void Load() => OnPropertyChanged(nameof(Paths));

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.Paths = new PathsSettings();
            Load();
        }
    }
}