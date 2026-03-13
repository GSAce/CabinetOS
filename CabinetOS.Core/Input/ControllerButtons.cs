namespace CabinetOS.Core.Input
{
    public enum ControllerButton
    {
        None = 0,

        // Face buttons
        A,
        B,
        X,
        Y,

        // D-Pad
        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,

        // Shoulders
        LeftBumper,
        RightBumper,

        // Triggers
        LeftTrigger,
        RightTrigger,

        // Stick clicks (L3/R3)
        LeftStick,
        RightStick,

        // Menu buttons (ES-style)
        View,   // Select
        Menu,   // Start

        // Optional / system
        Guide,
        Share
    }
}