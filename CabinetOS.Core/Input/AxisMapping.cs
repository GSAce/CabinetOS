namespace CabinetOS.Core.Input
{
    public readonly struct AxisMapping
    {
        public ControllerAxis Axis { get; }
        public AxisDirection Direction { get; }

        public AxisMapping(ControllerAxis axis, AxisDirection direction)
        {
            Axis = axis;
            Direction = direction;
        }

        public override string ToString()
            => $"{Axis} ({Direction})";
    }
}