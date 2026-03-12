using System.Windows;

namespace CabinetOS.UI
{
    public partial class ShellWindow : Window
    {
        public static ShellWindow? Instance { get; set; }
        private bool _menuOpen = false;

        public ShellWindow()
        {
            Instance = this;
            InitializeComponent();

            SystemBar.MainMenuRequested += ToggleMainMenu;
            SystemBar.HomeRequested += ShowWelcomeScreen;
            SystemBar.BackRequested += HandleBackNavigation;
            ShowWelcomeScreen();
        }

        private void ToggleMainMenu()
        {
            if (_menuOpen)
            {
                HideMainMenu();
            }
            else
            {
                MainMenu.Visibility = Visibility.Visible;
                _menuOpen = true;
            }
        }

        private void HideMainMenu()
        {
            MainMenu.Visibility = Visibility.Collapsed;
            _menuOpen = false;
        }

        public void ShowWelcomeScreen()
        {
            HideMainMenu();
            ContentHost.Content = new WelcomeScreen();
            SystemBar.SetBackButtonVisible(false); // Home screen
        }

        public void ShowHomeScreen()
        {
            HideMainMenu();
            ContentHost.Content = new HomeScreen();
            SystemBar.SetBackButtonVisible(true);
        }

        public void ShowArcadeTools()
        {
            HideMainMenu();
            ContentHost.Content = new ArcadeToolsScreen();
            SystemBar.SetBackButtonVisible(true);
        }

        public void ShowSettingsHomeScreen()
        {
            HideMainMenu();
            ContentHost.Content = new Settings.SettingsHomeScreen();
            SystemBar.SetBackButtonVisible(true);
        }

        
            private void HandleBackNavigation()
        {
            switch (ContentHost.Content)
            {
                // If you're on the Settings home screen, go back to the main Home screen
                case Settings.SettingsHomeScreen:
                    ShowWelcomeScreen();
                    break;

                // If you're inside any nested settings screen, go back to Settings home
                case Settings.DisplaySettingsScreen:
                case Settings.AudioSettingsScreen:
                case Settings.NetworkSettingsScreen:
                case Settings.RetroLibrarySettingsScreen:
                case Settings.StorageSettingsScreen:
                case Settings.AboutSettingsScreen:
                    ShowSettingsHomeScreen();
                    break;

                // Default fallback: go home
                default:
                    ShowWelcomeScreen();
                    break;
            }
        }
    }
}