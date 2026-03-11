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

        // TODO make settings screen
        //public void ShowSettings()
        //{
        //    HideMainMenu();
        //    ContentHost.Content = new SettingsScreen();
        //    SystemBar.SetBackButtonVisible(true);
        //}
    }
}