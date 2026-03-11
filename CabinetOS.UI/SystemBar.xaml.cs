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

        public SystemBar()
        {
            InitializeComponent();

            // Initialize and start the clock timer
            _clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            // Set initial time immediately
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

        public void SetCenterStatus(string text)
        {
            CenterStatusText.Text = text;
        }
    }
}