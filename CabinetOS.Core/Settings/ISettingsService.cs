using System.Threading.Tasks;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.Core.Settings
{
    public interface ISettingsService
    {
        CabinetSettings Current { get; }

        Task LoadAsync();
        Task SaveAsync();
    }
}