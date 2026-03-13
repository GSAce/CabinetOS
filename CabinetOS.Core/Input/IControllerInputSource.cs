using System.Threading;
using System.Threading.Tasks;

namespace CabinetOS.Core.Input
{
    public interface IControllerInputSource
    {
        bool IsControllerConnected(int controllerIndex);

        Task<ControllerButton> WaitForNextButtonAsync(
            int controllerIndex,
            CancellationToken cancellationToken = default);

        Task<AxisMapping> WaitForNextAxisAsync(
            int controllerIndex,
            float threshold,
            CancellationToken cancellationToken = default);
    }
}