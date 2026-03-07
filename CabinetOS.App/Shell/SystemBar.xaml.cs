using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CabinetOS.App.Shell
{
    public partial class SystemBar : UserControl
    {
        private readonly DispatcherTimer _clockTimer;

        // Typed reference to MainWindow
        private MainWindow Main => (MainWindow)Application.Current.MainWindow;

        public SystemBar()
        {
            InitializeComponent();

            // CLOCK TIMER
            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _clockTimer.Tick += (s, e) =>
            {
                ClockText.Text = DateTime.Now.ToString("HH:mm");
            };

            _clockTimer.Start();
        }

        // ---------------------------------------
        // NETWORK MODE ICON HANDLING
        // ---------------------------------------
        public void SetNetworkMode(string mode, int wifiStrength = 0)
        {
            DirectLinkIcon.Visibility = Visibility.Collapsed;
            LanIcon.Visibility = Visibility.Collapsed;
            WifiIcon.Visibility = Visibility.Collapsed;

            switch (mode)
            {
                case "DirectLink":
                    DirectLinkIcon.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Icons/DirectLinkIcon.png"));
                    DirectLinkIcon.Visibility = Visibility.Visible;
                    break;

                case "Lan":
                    LanIcon.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Icons/Lan_Icon.png"));
                    LanIcon.Visibility = Visibility.Visible;
                    break;

                case "WiFi":
                    WifiIcon.Source = new BitmapImage(
                        new Uri($"pack://application:,,,/Resources/Icons/WiFi_{wifiStrength}_Icon.png"));
                    WifiIcon.Visibility = Visibility.Visible;
                    break;

                default:
                    WifiIcon.Source = new BitmapImage(
                        new Uri("pack://application:,,,/Resources/Icons/WiFi_0_Icon.png"));
                    WifiIcon.Visibility = Visibility.Visible;
                    break;
            }
        }

        // ---------------------------------------
        // SIDEBAR TOGGLE
        // ---------------------------------------
        private void StatusToggle_Checked(object sender, RoutedEventArgs e)
        {
            Main.StatusSideBar.Open();
        }

        private void StatusToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Main.StatusSideBar.Close();
        }

        private void StatusToggleIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.ToggleStatusSideBar();
        }

        // ---------------------------------------
        // HOME BUTTON
        // ---------------------------------------
        private void HomeIcon_Click(object sender, MouseButtonEventArgs e)
        {
            Main.NavigateToWelcomeScreen();
        }

        // ---------------------------------------
        // BACK BUTTON
        // ---------------------------------------
        private void BackIcon_Click(object sender, MouseButtonEventArgs e)
        {
            Main.NavigateBack();
        }
    }
}