using System.Windows.Controls;
using CabinetOS.App;

namespace CabinetOS.App.Services
{
    public class NavigationService
    {
        public string CurrentModuleName { get; private set; } = "Welcome";

        public void Navigate(UserControl module, string moduleName)
        {
            CurrentModuleName = moduleName;

            MainWindow.Instance.ShowModule(module);
            MainWindow.Instance.SetModuleName(moduleName);
        }

        public void BackToWelcome()
        {
            MainWindow.Instance.ShowWelcomeScreen(true);
            MainWindow.Instance.SetModuleName("Welcome");
        }
    }
}