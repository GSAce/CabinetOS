using System.Collections.Generic;
using System.Windows.Controls;
using CabinetOS.App.Shell;

namespace CabinetOS.App
{
    public static class ModuleManager
    {
        private static readonly Stack<UserControl> _history = new Stack<UserControl>();

        public static UserControl? CurrentModule { get; set; } = default!;

        public static bool CanGoBack => _history.Count > 0;

        public static void LoadModule(string moduleName)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            UserControl module = moduleName switch
            {
                "WelcomeScreen" => new WelcomeScreen(),

                // Add more screens here later:
                // "Settings" => new SettingsScreen(),
                // "Games" => new GamesScreen(),

                _ => null
            };
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (module == null)
                return;

            if (CurrentModule != null)
                _history.Push(CurrentModule);

            CurrentModule = module;
            MainWindow.Instance.SetModule(module);
        }

        public static void GoBack()
        {
            if (!CanGoBack)
                return;

            CurrentModule = _history.Pop();
            MainWindow.Instance.SetModule(CurrentModule);
        }
    }
}