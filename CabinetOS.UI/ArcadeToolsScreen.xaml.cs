using System;
using System.Windows;
using System.Windows.Controls;

namespace CabinetOS.UI
{
    public partial class ArcadeToolsScreen : UserControl
    {
        public event Action? ArcadeScanSortRequested;

        public ArcadeToolsScreen()
        {
            InitializeComponent();
        }

        private void StartScanSort_Click(object sender, RoutedEventArgs e)
        {
            Log("Starting Arcade Scan & Sort...");
            StatusLabel.Text = "Processing...";
            Progress.Value = 0;

            ArcadeScanSortRequested?.Invoke();
        }

        // Backend calls this to update progress
        public void SetProgress(double percent)
        {
            Progress.Value = percent;
        }

        // Backend calls this to update status
        public void SetStatus(string text)
        {
            StatusLabel.Text = text;
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new WelcomeScreen();
        }
        // Backend calls this to append log lines
        public void Log(string message)
        {
            LogOutput.Text += $"{DateTime.Now:HH:mm:ss}  {message}\n";
        }
        public void ShowSummary(int totalRoms, int validSets, int missingSets, int unknownFiles, TimeSpan duration)
        {
            SummaryTotalRoms.Text = $"Total ROMs Scanned: {totalRoms}";
            SummaryValidSets.Text = $"Valid Sets: {validSets}";
            SummaryMissingSets.Text = $"Missing Sets: {missingSets}";
            SummaryUnknownFiles.Text = $"Unknown Files: {unknownFiles}";
            SummaryTimeTaken.Text = $"Time Taken: {duration:mm\\:ss}";

            SummaryPanel.Visibility = Visibility.Visible;
        }

        private void CloseSummary_Click(object sender, RoutedEventArgs e)
        {
            SummaryPanel.Visibility = Visibility.Collapsed;
        }
    }
}