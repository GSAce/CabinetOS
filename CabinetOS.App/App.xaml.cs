using System.Windows;
using CabinetOS.Core.Metadata;

namespace CabinetOS.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // TEMPORARY ROMS PATH
            MetadataService.Initialize(@"H:\roms backup");
        }
    }
}