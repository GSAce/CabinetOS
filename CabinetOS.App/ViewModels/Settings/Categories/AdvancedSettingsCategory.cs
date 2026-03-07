using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class AdvancedSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "Advanced";

        private readonly AdvancedSettingsView _view = new AdvancedSettingsView();
        public object View => _view;

        public void Load() { }
        public void Save() { }
        public void ResetToDefaults() { }
    }
}