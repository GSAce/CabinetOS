using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CabinetOS.UI
{
    public partial class WelcomeScreen : UserControl
    {
        public WelcomeScreen()
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ShowSettingsHomeScreen();
        }
    }
    
}