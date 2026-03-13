namespace CabinetOS.UI
{
    public static class InputListener
    {
        public static event Action<string>? OnButtonPressed;

        public static void StartListening() { }
        public static void StopListening() { }
    }
}