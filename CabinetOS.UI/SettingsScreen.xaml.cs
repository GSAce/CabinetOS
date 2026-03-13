using System.Windows;
using System.Windows.Controls;
using CabinetOS.Core.Input;
using CabinetOS.UI.Windows;

namespace CabinetOS.UI
{
    public partial class SettingsScreen : UserControl
    {
        private readonly ControllerMapping _inputMapper;
        private readonly ControllerMappingStore _mappingStore;

        public SettingsScreen(ControllerMapping inputMapper, ControllerMappingStore mappingStore)
        {
            InitializeComponent();
            _inputMapper = inputMapper;
            _mappingStore = mappingStore;

            // Force visibility ON
            ControllerSettingsButton.Visibility = Visibility.Visible;
        }

        private void ControllerSettings_Click(object sender, RoutedEventArgs e)
        {
            var window = new ControllerMappingWindow(
                _inputMapper,
                _mappingStore,
                controllerIndex: 0,
                actionName: "A Button",
                isAxis: false);

            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }
    }
}