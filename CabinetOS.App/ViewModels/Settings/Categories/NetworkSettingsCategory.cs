using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class NetworkSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "Network";

        private readonly NetworkSettingsView _view = new NetworkSettingsView();
        public object View => _view;

        public void Load() { }
        public void Save() { }
        public void ResetToDefaults() { }
    }
}