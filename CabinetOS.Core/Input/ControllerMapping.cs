using System;
using System.Threading;
using System.Threading.Tasks;

namespace CabinetOS.Core.Input
{
    public sealed class ControllerMapping
    {
        private readonly IControllerInputSource _inputSource;

        public ControllerMapping(IControllerInputSource inputSource)
        {
            _inputSource = inputSource ?? throw new ArgumentNullException(nameof(inputSource));
        }

        public async Task<ControllerButton> MapButtonAsync(
            int controllerIndex,
            TimeSpan? timeout = null,
            TimeSpan? skipHoldTime = null,
            CancellationToken cancellationToken = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            if (timeout.HasValue && timeout.Value > TimeSpan.Zero)
                cts.CancelAfter(timeout.Value);

            skipHoldTime ??= TimeSpan.FromSeconds(1);

            var token = cts.Token;
            var holdStart = DateTime.MinValue;
            ControllerButton lastButton = ControllerButton.None;

            while (!token.IsCancellationRequested)
            {
                var button = await _inputSource
                    .WaitForNextButtonAsync(controllerIndex, token)
                    .ConfigureAwait(false);

                if (button == ControllerButton.None)
                    continue;

                if (lastButton == ControllerButton.None)
                {
                    lastButton = button;
                    holdStart = DateTime.UtcNow;
                }
                else if (button == lastButton)
                {
                    if (DateTime.UtcNow - holdStart >= skipHoldTime.Value)
                        return ControllerButton.None;
                }
                else
                {
                    return button;
                }
            }

            return ControllerButton.None;
        }

        public async Task<AxisMapping> MapAxisAsync(
            int controllerIndex,
            float threshold = 0.5f,
            TimeSpan? timeout = null,
            TimeSpan? skipHoldTime = null,
            CancellationToken cancellationToken = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            if (timeout.HasValue && timeout.Value > TimeSpan.Zero)
                cts.CancelAfter(timeout.Value);

            skipHoldTime ??= TimeSpan.FromSeconds(1);

            var token = cts.Token;
            var holdStart = DateTime.MinValue;
            AxisMapping lastAxis = new AxisMapping(ControllerAxis.None, AxisDirection.None);

            while (!token.IsCancellationRequested)
            {
                var axis = await _inputSource
                    .WaitForNextAxisAsync(controllerIndex, threshold, token)
                    .ConfigureAwait(false);

                if (axis.Axis == ControllerAxis.None)
                    continue;

                if (lastAxis.Axis == ControllerAxis.None)
                {
                    lastAxis = axis;
                    holdStart = DateTime.UtcNow;
                }
                else if (axis.Axis == lastAxis.Axis && axis.Direction == lastAxis.Direction)
                {
                    if (DateTime.UtcNow - holdStart >= skipHoldTime.Value)
                        return new AxisMapping(ControllerAxis.None, AxisDirection.None);
                }
                else
                {
                    return axis;
                }
            }

            return new AxisMapping(ControllerAxis.None, AxisDirection.None);
        }
    }
}