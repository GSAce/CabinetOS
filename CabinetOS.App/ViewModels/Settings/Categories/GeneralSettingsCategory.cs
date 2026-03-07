using CabinetOS.App.Views.Settings.Categories;

namespace CabinetOS.App.ViewModels.Settings.Categories
{
    public class GeneralSettingsCategory : ISettingsCategory
    {
        public string DisplayName => "General";

        private readonly GeneralSettingsCategoryView _view = new GeneralSettingsCategoryView();
        public object View => _view;

        public void Load()
        {
            // TODO: Load general settings
        }

        public void Save()
        {
            // TODO: Save general settings
        }

        public void ResetToDefaults()
        {
            // TODO: Reset general settings
        }
    }
}