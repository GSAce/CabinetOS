using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CabinetOS.UI
{
    public partial class SystemBar : UserControl
    {
        private readonly DispatcherTimer _clockTimer;

        public event Action? MainMenuRequested;
        public event Action? HomeRequested;
        public event Action? BackRequested;

        public SystemBar()
        {
            InitializeComponent();

            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            ClockText.Text = DateTime.Now.ToString("h:mm tt");
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            ClockText.Text = DateTime.Now.ToString("h:mm tt");
        }

        private void CabinetOS_Click(object sender, RoutedEventArgs e)
        {
            MainMenuRequested?.Invoke();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke();
        }

        public void SetBackButtonVisible(bool isVisible)
        {
            BackButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetCenterStatus(string text)
        {
            CenterStatusText.Text = text;
        }
    }
}