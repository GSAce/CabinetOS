using System.Collections.ObjectModel;
using System.Windows.Input;
using CabinetOS.App.ViewModels.Settings.Categories;

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

        public SettingsViewModel()
        {
            Categories = new ObservableCollection<ISettingsCategory>
            {
                new GeneralSettingsCategory(),
                new AppearanceSettingsCategory(),
                new NetworkSettingsCategory(),
                new PathsSettingsCategory(),
                new ScraperSettingsCategory(),
                new AdvancedSettingsCategory(),
                new AboutSettingsCategory()
            };

            SelectedCategory = Categories.Count > 0 ? Categories[0] : null;

            SaveCommand = new RelayCommand(_ => SelectedCategory?.Save());
            LoadCommand = new RelayCommand(_ => SelectedCategory?.Load());
            ResetCommand = new RelayCommand(_ => SelectedCategory?.ResetToDefaults());
        }
    }
}