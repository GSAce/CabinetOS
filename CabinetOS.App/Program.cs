//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Windows;
//using CabinetOS.UI;

//namespace CabinetOS.App
//{
//    public static class Program
//    {
//        [STAThread]
//        public static void Main()
//        {
//            var services = new ServiceCollection();

//            // DI will be added later

//            var provider = services.BuildServiceProvider();

//            var app = new App();
//            app.InitializeComponent();

//            var shell = new ShellWindow();
//            app.Run(shell);
//        }
//    }
//}