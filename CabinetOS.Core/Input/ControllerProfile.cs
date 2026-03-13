using CabinetOS.Core.Input;

public sealed class ControllerProfile
{
    // Hotkey + Start
    public ControllerButton HotkeyButton { get; set; } = ControllerButton.View;
    public ControllerButton StartButton { get; set; } = ControllerButton.Menu;

    // Face buttons
    public ControllerButton A { get; set; }
    public ControllerButton B { get; set; }
    public ControllerButton X { get; set; }
    public ControllerButton Y { get; set; }

    // D-Pad
    public ControllerButton DPadUp { get; set; }
    public ControllerButton DPadDown { get; set; }
    public ControllerButton DPadLeft { get; set; }
    public ControllerButton DPadRight { get; set; }

    // Shoulders
    public ControllerButton LeftBumper { get; set; }
    public ControllerButton RightBumper { get; set; }

    // Triggers (button interpretation)
    public ControllerButton LeftTriggerButton { get; set; }
    public ControllerButton RightTriggerButton { get; set; }

    // Stick clicks
    public ControllerButton LeftStickClick { get; set; }
    public ControllerButton RightStickClick { get; set; }

    // AXIS MAPPINGS (EmulationStation style)
    public AxisMapping LeftStickUp { get; set; }
    public AxisMapping LeftStickDown { get; set; }
    public AxisMapping LeftStickLeft { get; set; }
    public AxisMapping LeftStickRight { get; set; }

    public AxisMapping RightStickUp { get; set; }
    public AxisMapping RightStickDown { get; set; }
    public AxisMapping RightStickLeft { get; set; }
    public AxisMapping RightStickRight { get; set; }

    public AxisMapping LeftTriggerAxis { get; set; }
    public AxisMapping RightTriggerAxis { get; set; }
}