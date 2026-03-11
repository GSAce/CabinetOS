using System;
using System.Windows;

namespace CabinetOS.UI
{
    public partial class ShellWindow : Window
    {
        public static ShellWindow Instance { get; set; }

        public ShellWindow()
        {
            Instance = this;
            InitializeComponent();

            // Load WelcomeScreen as the default screen
            ContentHost.Content = new WelcomeScreen();

            // Hook SystemBar events
            SystemBar.MainMenuRequested += ShowMainMenu;
            SystemBar.HomeRequested += ShowHomeScreen;
        }

        public void ShowHomeScreen()
        {
            ContentHost.Content = new HomeScreen();
            HideMainMenu();
        }

        public void ShowArcadeTools()
        {
            ContentHost.Content = new ArcadeToolsScreen();
            HideMainMenu();
        }

        // ⭐ NEW: Show the slide-out menu instead of replacing the screen
        public void ShowMainMenu()
        {
            if (MainMenu.Visibility == Visibility.Visible)
                HideMainMenu();
            else
                MainMenu.Visibility = Visibility.Visible;
        }

        // ⭐ NEW: Hide the slide-out menu
        public void HideMainMenu()
        {
            MainMenu.Visibility = Visibility.Collapsed;
        }
    }
}