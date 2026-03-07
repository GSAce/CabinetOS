using CabinetOS.Core.Models;
using CabinetOS.Core.Utilities;

namespace CabinetOS.Core.Services
{
    public static class RomScannerService
    {
        public static List<RomInfo> ScanFolder(string folderPath)
        {
            PlatformDatabase.EnsureLoaded();

            List<RomInfo> results = new();

            if (!Directory.Exists(folderPath))
                return results;

            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToLower();

                // Find platform by extension
                var platform = PlatformDatabase.GetPlatformByExtension(ext);

                if (platform == null)
                    continue; // Not a ROM we care about

                results.Add(new RomInfo
                {
                    FilePath = file,
                    FileName = Path.GetFileName(file),
                    Extension = ext,
                    Platform = platform
                });
            }

            return results;
        }
    }
}