using System.Windows;
using System.Windows.Controls;

namespace CabinetOS.UI
{
    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewerBehavior),
                new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

        public static void SetHorizontalOffset(DependencyObject element, double value)
        {
            element.SetValue(HorizontalOffsetProperty, value);
        }

        public static double GetHorizontalOffset(DependencyObject element)
        {
            return (double)element.GetValue(HorizontalOffsetProperty);
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer viewer)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }
}