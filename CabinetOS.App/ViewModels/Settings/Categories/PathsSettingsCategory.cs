using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class PathsSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "Paths";

        private readonly PathsSettingsView _view = new PathsSettingsView();
        public object View => _view;

        public void Load() { }
        public void Save() { }
        public void ResetToDefaults() { }
    }
}