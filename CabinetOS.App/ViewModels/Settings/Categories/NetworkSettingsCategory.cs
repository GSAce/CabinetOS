using CabinetOS.App.Views.Settings.Categories;
using CabinetOS.Core.Settings;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class NetworkSettingsCategory : ViewModelBase, ISettingsCategory
    {
        public string DisplayName => "Network";

        private readonly NetworkSettingsView _view = new NetworkSettingsView();
        public object View => _view;

        private readonly SettingsService _settingsService;

        public NetworkSettingsCategory(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public NetworkSettings Network
        {
            get => _settingsService.Current.Network;
            set
            {
                _settingsService.Current.Network = value;
                OnPropertyChanged();
            }
        }

        public void Load() => OnPropertyChanged(nameof(Network));

        public async void Save() => await _settingsService.SaveAsync();

        public void ResetToDefaults()
        {
            _settingsService.Current.Network = new NetworkSettings();
            Load();
        }
    }
}