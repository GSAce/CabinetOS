using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CabinetOS.Core.Input;

namespace CabinetOS.UI.Input
{
    public sealed class XInputControllerInputSource : IControllerInputSource
    {
        // XInput constants
        private const int ERROR_SUCCESS = 0;

        // XInput structures
        [StructLayout(LayoutKind.Sequential)]
        private struct XINPUT_STATE
        {
            public uint dwPacketNumber;
            public XINPUT_GAMEPAD Gamepad;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct XINPUT_GAMEPAD
        {
            public ushort wButtons;
            public byte bLeftTrigger;
            public byte bRightTrigger;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
        }

        // XInput import
        [DllImport("xinput1_4.dll", EntryPoint = "XInputGetState")]
        private static extern int XInputGetState(int dwUserIndex, out XINPUT_STATE pState);

        public bool IsControllerConnected(int controllerIndex)
        {
            return XInputGetState(controllerIndex, out _) == ERROR_SUCCESS;
        }

        public async Task<ControllerButton> WaitForNextButtonAsync(
            int controllerIndex,
            CancellationToken cancellationToken = default)
        {
            XINPUT_STATE prevState;
            XInputGetState(controllerIndex, out prevState);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10, cancellationToken);

                if (XInputGetState(controllerIndex, out var state) != ERROR_SUCCESS)
                    continue;

                ushort changed = (ushort)(state.Gamepad.wButtons & ~prevState.Gamepad.wButtons);

                if (changed != 0)
                {
                    return MapXInputButton(changed);
                }

                prevState = state;
            }

            return ControllerButton.None;
        }

        public async Task<AxisMapping> WaitForNextAxisAsync(
            int controllerIndex,
            float threshold,
            CancellationToken cancellationToken = default)
        {
            const float MAX_AXIS = 32767f;

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(10, cancellationToken);

                if (XInputGetState(controllerIndex, out var state) != ERROR_SUCCESS)
                    continue;

                // Left Stick X
                if (Math.Abs(state.Gamepad.sThumbLX) / MAX_AXIS >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.LeftStickX,
                        state.Gamepad.sThumbLX > 0 ? AxisDirection.Positive : AxisDirection.Negative
                    );
                }

                // Left Stick Y
                if (Math.Abs(state.Gamepad.sThumbLY) / MAX_AXIS >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.LeftStickY,
                        state.Gamepad.sThumbLY > 0 ? AxisDirection.Positive : AxisDirection.Negative
                    );
                }

                // Right Stick X
                if (Math.Abs(state.Gamepad.sThumbRX) / MAX_AXIS >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.RightStickX,
                        state.Gamepad.sThumbRX > 0 ? AxisDirection.Positive : AxisDirection.Negative
                    );
                }

                // Right Stick Y
                if (Math.Abs(state.Gamepad.sThumbRY) / MAX_AXIS >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.RightStickY,
                        state.Gamepad.sThumbRY > 0 ? AxisDirection.Positive : AxisDirection.Negative
                    );
                }

                // Left Trigger
                if (state.Gamepad.bLeftTrigger / 255f >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.LeftTrigger,
                        AxisDirection.Positive
                    );
                }

                // Right Trigger
                if (state.Gamepad.bRightTrigger / 255f >= threshold)
                {
                    return new AxisMapping(
                        ControllerAxis.RightTrigger,
                        AxisDirection.Positive
                    );
                }
            }

            return new AxisMapping(ControllerAxis.None, AxisDirection.None);
        }

        private ControllerButton MapXInputButton(ushort button)
        {
            return button switch
            {
                0x1000 => ControllerButton.A,
                0x2000 => ControllerButton.B,
                0x4000 => ControllerButton.X,
                0x8000 => ControllerButton.Y,

                0x0001 => ControllerButton.DPadUp,
                0x0002 => ControllerButton.DPadDown,
                0x0004 => ControllerButton.DPadLeft,
                0x0008 => ControllerButton.DPadRight,

                0x0100 => ControllerButton.LeftBumper,
                0x0200 => ControllerButton.RightBumper,

                0x0040 => ControllerButton.LeftStick,
                0x0080 => ControllerButton.RightStick,

                0x0010 => ControllerButton.View,
                0x0020 => ControllerButton.Menu,

                _ => ControllerButton.None
            };
        }
    }
}