using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CabinetOS.Core.Arcade.DatDiscovery
{
    public static class ArcadeDatScanner
    {
        public static IReadOnlyList<ArcadeDatFamilyInfo> Scan(string datFilesRoot)
        {
            var results = new List<ArcadeDatFamilyInfo>();

            if (!Directory.Exists(datFilesRoot))
                return results;

            foreach (var familyDir in Directory.GetDirectories(datFilesRoot))
            {
                var familyName = Path.GetFileName(familyDir);

                // Collect ALL .dat and .xml files
                var datFiles = Directory.GetFiles(familyDir, "*.dat")
                    .Concat(Directory.GetFiles(familyDir, "*.xml"))
                    .ToList();

                if (datFiles.Count == 0)
                    continue;

                results.Add(new ArcadeDatFamilyInfo(familyName, datFiles));
            }

            return results;
        }
    }
}