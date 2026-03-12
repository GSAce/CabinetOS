using System.Windows;
using System.Windows.Controls;

namespace CabinetOS.UI.Settings
{
    public partial class SettingsHomeScreen : UserControl
    {
        public SettingsHomeScreen()
        {
            InitializeComponent();
        }

        private void Display_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new DisplaySettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }

        private void Audio_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new AudioSettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }

        private void Network_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new NetworkSettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }

        private void RetroLibrary_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new RetroLibrarySettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }

        private void Storage_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new StorageSettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new AboutSettingsScreen();
            ShellWindow.Instance.SystemBar.SetBackButtonVisible(true);
        }
    }
}