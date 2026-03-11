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
            ShellWindow.Instance.ContentHost.Content = new ArcadeToolsScreen();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder until SettingsScreen exists
            MessageBox.Show("Settings screen not implemented yet.");
        }
        private void Home_Click(object sender, RoutedEventArgs e) { }
        private void Scanner_Click(object sender, RoutedEventArgs e) { }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}