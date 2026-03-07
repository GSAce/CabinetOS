using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CabinetOS.App.Shell;
using CabinetOS.App.Services;

namespace CabinetOS.App
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; } = null!;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Start network status timer
            DispatcherTimer networkTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            networkTimer.Tick += (s, e) => UpdateNetworkStatus();
            networkTimer.Start();
        }

        // ---------------------------------------------------------
        // NETWORK STATUS
        // ---------------------------------------------------------
        public void UpdateNetworkStatus()
        {
            bool directLinkActive = NetworkManager.IsDirectLinkConnected();
            bool lanActive = NetworkManager.IsLanConnected();
            bool wifiActive = NetworkManager.IsWifiConnected();
            int wifiStrength = NetworkManager.GetWifiStrength(); // 0–4

            if (directLinkActive)
                SystemBarControl.SetNetworkMode("DirectLink");
            else if (lanActive)
                SystemBarControl.SetNetworkMode("Lan");
            else if (wifiActive)
                SystemBarControl.SetNetworkMode("WiFi", wifiStrength);
            else
                SystemBarControl.SetNetworkMode("None");
        }

        // ---------------------------------------------------------
        // SIDEBAR CONTROL
        // ---------------------------------------------------------
        public void ToggleStatusSideBar()
        {
            StatusSideBarRegion.Visibility =
                StatusSideBarRegion.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        // Expose the actual sidebar instance
        public StatusSideBar StatusSideBar => StatusSideBarControl;

        // ---------------------------------------------------------
        // MODULE HOSTING
        // ---------------------------------------------------------
        public void SetModule(UserControl module)
        {
            ModuleHost.Content = module;
        }

        public void ShowModule(UserControl module)
        {
            ContentRegion.Children.Clear();
            ContentRegion.Children.Add(module);
        }

        public void ShowWelcomeScreen(bool show)
        {
            WelcomeScreenOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        // ---------------------------------------------------------
        // SYSTEM BAR TITLE
        // ---------------------------------------------------------
        public void SetModuleName(string name)
        {
            SystemBarControl.ModuleNameText.Text = name;
        }

        // ---------------------------------------------------------
        // NAVIGATION
        // ---------------------------------------------------------
        public void NavigateToWelcomeScreen()
        {
            ModuleManager.LoadModule("WelcomeScreen");
        }

        public void NavigateBack()
        {
            if (ModuleManager.CanGoBack)
                ModuleManager.GoBack();
        }
    }
}