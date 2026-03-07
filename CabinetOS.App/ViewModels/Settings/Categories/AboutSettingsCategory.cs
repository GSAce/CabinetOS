using CabinetOS.App.Views.Settings;
using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class AboutSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "About";

        private readonly AboutSettingsView _view = new AboutSettingsView();
        public object View => _view;

        public void Load() { }
        public void Save() { }
        public void ResetToDefaults() { }
    }
}