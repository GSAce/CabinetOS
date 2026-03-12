using System.Windows;
using System.Windows.Controls;

namespace CabinetOS.UI
{
    public partial class MainMenuScreen : UserControl
    {
        public MainMenuScreen()
        {
            InitializeComponent();
        }

        private void RetroLibrary_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ShowHomeScreen();
        }

        private void ArcadeTools_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ShowArcadeTools();
        }

        // TODO make settings screen
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ShowSettingsHomeScreen();


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}