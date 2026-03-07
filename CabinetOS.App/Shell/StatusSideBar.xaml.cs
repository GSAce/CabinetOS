using System.Windows;
using System.Windows.Controls;

namespace CabinetOS.App.Shell
{
    public partial class StatusSideBar : UserControl
    {
        public StatusSideBar()
        {
            InitializeComponent();
        }

        public void Open()
        {
            // Show the sidebar region in MainWindow
            var main = (MainWindow)Application.Current.MainWindow;
            main.StatusSideBarRegion.Visibility = Visibility.Visible;
        }

        public void Close()
        {
            // Hide the sidebar region in MainWindow
            var main = (MainWindow)Application.Current.MainWindow;
            main.StatusSideBarRegion.Visibility = Visibility.Collapsed;
        }
    }
}