using System;
using System.Threading;
using System.Threading.Tasks;

namespace CabinetOS.Core.Input
{
    public sealed class ControllerIdentifier
    {
        private readonly IControllerInputSource _inputSource;

        public ControllerIdentifier(IControllerInputSource inputSource)
        {
            _inputSource = inputSource ?? throw new ArgumentNullException(nameof(inputSource));
        }

        /// <summary>
        /// Waits for the user to hold the A button on any connected controller.
        /// Returns the detected controller index, or -1 if cancelled/timeout.
        /// </summary>
        public async Task<int> IdentifyControllerAsync(
            int maxControllers = 4,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if (timeout.HasValue && timeout.Value > TimeSpan.Zero)
                cts.CancelAfter(timeout.Value);

            var token = cts.Token;

            // Simple polling loop: try each controller index, wait for A.
            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < maxControllers; i++)
                {
                    if (!_inputSource.IsControllerConnected(i))
                        continue;

                    var button = await _inputSource.WaitForNextButtonAsync(i, token)
                                                   .ConfigureAwait(false);

                    if (button == ControllerButton.A)
                        return i;
                }
            }

            return -1;
        }
    }
}