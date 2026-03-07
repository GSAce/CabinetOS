using System.Windows.Controls;
using System.Windows.Input;
using CabinetOS.App.Services;

namespace CabinetOS.App.Shell
{
    public partial class WelcomeScreen : UserControl
    {
        private readonly NavigationService _nav = new NavigationService();

        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void Designer_Click(object sender, MouseButtonEventArgs e)
        {
            _nav.Navigate(new Modules.Designer.DesignerView(), "Designer");
        }

        private void Mapping_Click(object sender, MouseButtonEventArgs e)
        {
            _nav.Navigate(new Modules.Mapping.MappingView(), "Mapping");
        }

        private void Bezel_Click(object sender, MouseButtonEventArgs e)
        {
            _nav.Navigate(new Modules.Bezel.BezelView(), "Bezel");
        }

        private void PiSync_Click(object sender, MouseButtonEventArgs e)
        {
            _nav.Navigate(new Modules.PiSync.PiSyncView(), "Pi Sync");
        }

        private void Tools_Click(object sender, MouseButtonEventArgs e)
        {
            _nav.Navigate(new Modules.Tools.ToolsView(), "Tools");
        }
    }
}