using System;
using System.Windows;
using System.Windows.Controls;
using CabinetOS.Core.Metadata;

namespace CabinetOS.UI
{
    public partial class HomeScreen : UserControl
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void CarouselLoaded(object sender, RoutedEventArgs e)
        {
            Carousel.Focus();
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            ShellWindow.Instance.ContentHost.Content = new WelcomeScreen();
        }
        // -------------------------
        // SELECTED SYSTEM METADATA
        // -------------------------
        public SystemMetadata? SelectedSystemMetadata
        {
            get => (SystemMetadata?)GetValue(SelectedSystemMetadataProperty);
            set => SetValue(SelectedSystemMetadataProperty, value);
        }

        public static readonly DependencyProperty SelectedSystemMetadataProperty =
            DependencyProperty.Register(nameof(SelectedSystemMetadata),
                typeof(SystemMetadata),
                typeof(HomeScreen),
                new PropertyMetadata(null));

        // -------------------------
        // CAROUSEL SELECTION CHANGED
        // -------------------------
        private void Carousel_SelectionChanged(object? sender, EventArgs e)
        {
            if (Carousel.SelectedItem is not SystemMetadata meta)
                return;

            // Directly assign metadata object
            SelectedSystemMetadata = meta;
        }

        public void FocusCarousel()
        {
            Carousel.Focus();
        }
    }
}