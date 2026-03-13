using System.Windows.Controls;
using CabinetOS.Core.Input;

namespace CabinetOS.UI.Controls
{
    public partial class ControllerMappingView : UserControl
    {
        public ControllerMappingView()
        {
            InitializeComponent();
        }

        // Called by mapping window to highlight a button
        public void HighlightButton(ControllerButton button)
        {
            // Placeholder — you will add visuals later
        }

        // Called by mapping window to highlight an axis
        public void HighlightAxis(AxisMapping axis)
        {
            // Placeholder — you will add visuals later
        }
    }
}