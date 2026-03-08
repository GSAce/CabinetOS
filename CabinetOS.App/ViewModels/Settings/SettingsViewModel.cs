using System.Collections.ObjectModel;
using System.Windows.Input;
using CabinetOS.App.ViewModels.Settings.Categories;
using CabinetOS.Core.Settings;

namespace CabinetOS.App.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<ISettingsCategory> Categories { get; }

        private ISettingsCategory? _selectedCategory;
        public ISettingsCategory? SelectedCategory
        {
            get => _selectedCategory;
            set => SetField(ref _selectedCategory, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ResetCommand { get; }

        private readonly SettingsService _settingsService;

        public SettingsViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;

            // Build category list with dependency injection
            Categories = new ObservableCollection<ISettingsCategory>
            {
                new GeneralSettingsCategory(_settingsService),
                new AppearanceSettingsCategory(_settingsService),
                new NetworkSettingsCategory(_settingsService),
                new PathsSettingsCategory(_settingsService),
                new ScraperSettingsCategory(_settingsService),
                new AdvancedSettingsCategory(_settingsService),
                new AboutSettingsCategory()
            };

            SelectedCategory = Categories.Count > 0 ? Categories[0] : null;

            SaveCommand = new RelayCommand(async _ => await _settingsService.SaveAsync());
            LoadCommand = new RelayCommand(_ => ReloadAllCategories());
            ResetCommand = new RelayCommand(_ => SelectedCategory?.ResetToDefaults());
        }

        private void ReloadAllCategories()
        {
            foreach (var category in Categories)
                category.Load();
        }
    }
}