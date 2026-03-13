using System.Threading.Tasks;
using System.Windows;
using CabinetOS.Core.Input;
using CabinetOS.UI.Controls;

namespace CabinetOS.UI.Windows
{
    public partial class ControllerMappingWindow : Window
    {
        private readonly ControllerMapping _inputMapper;
        private readonly ControllerMappingStore _mappingStore;
        private readonly int _controllerIndex;
        private readonly bool _isAxis;

        private readonly RemapOverlayViewModel _remapVM;
        private string _currentInputKey;

        public ControllerMappingWindow(
            ControllerMapping inputMapper,
            ControllerMappingStore mappingStore,
            int controllerIndex,
            string actionName,
            bool isAxis)
        {
            InitializeComponent();

            _inputMapper = inputMapper;
            _mappingStore = mappingStore;
            _controllerIndex = controllerIndex;
            _isAxis = isAxis;

            ActionTitle.Text = $"Map: {actionName}";
            _currentInputKey = actionName;

            _remapVM = new RemapOverlayViewModel();
            RemapOverlay.DataContext = _remapVM;

            _remapVM.OnCancel += () =>
            {
                RemapOverlay.Visibility = Visibility.Collapsed;
            };
        }

        private async void BeginRemap(string inputKey)
        {
            _currentInputKey = inputKey;
            _remapVM.CurrentInputName = inputKey;

            RemapOverlay.Visibility = Visibility.Visible;

            if (_isAxis)
                await BeginAxisMappingAsync();
            else
                await BeginButtonMappingAsync();
        }

        private async Task BeginButtonMappingAsync()
        {
            var result = await _inputMapper.MapButtonAsync(_controllerIndex);

            RemapOverlay.Visibility = Visibility.Collapsed;

            if (result != ControllerButton.None)
            {
                _mappingStore.SetMapping(
                    _controllerIndex,
                    _currentInputKey,
                    result.ToString(),
                    isAxis: false);
            }

            DialogResult = true;
            Close();
        }

        private async Task BeginAxisMappingAsync()
        {
            var result = await _inputMapper.MapAxisAsync(_controllerIndex);

            RemapOverlay.Visibility = Visibility.Collapsed;

            if (result.Axis != ControllerAxis.None)
            {
                _mappingStore.SetMapping(
                    _controllerIndex,
                    _currentInputKey,
                    result.ToString(),
                    isAxis: true);
            }

            DialogResult = true;
            Close();
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            BeginRemap(_currentInputKey);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}