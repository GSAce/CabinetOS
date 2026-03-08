using System;
using System.Threading;
using System.Threading.Tasks;

namespace CabinetOS.Core.RomScanning
{
    public interface IRomScanner
    {
        Task<RomScanResult> ScanAsync(
            string romRoot,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default);
    }
}