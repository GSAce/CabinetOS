using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class AppearanceSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "Appearance";

        private readonly AppearanceSettingsView _view = new AppearanceSettingsView();
        public object View => _view;

        public void Load() { }
        public void Save() { }
        public void ResetToDefaults() { }
    }
}