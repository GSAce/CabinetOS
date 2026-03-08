using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CabinetOS.Core.Models;

namespace CabinetOS.Core.RomScanning
{
    public class RomNameNormalizer
    {
        private static readonly Regex Bracketed = new(@"\(.*?\)|\[.*?\]", RegexOptions.Compiled);
        private static readonly Regex MultiSpace = new(@"\s{2,}", RegexOptions.Compiled);

        public string NormalizeDisplayName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);

            name = Bracketed.Replace(name, "");
            name = name.Replace("_", " ");
            name = MultiSpace.Replace(name, " ").Trim();

            if (!string.IsNullOrWhiteSpace(name))
                name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

            return name;
        }

        public void ApplyOnDisk(RomInfo rom)
        {
            if (rom.IsNormalized)
                return;

            var directory = Path.GetDirectoryName(rom.FilePath);
            if (directory == null)
                return;

            rom.OriginalName ??= rom.FileName;

            var normalizedName = NormalizeDisplayName(rom.FileName);
            var newPath = Path.Combine(directory, normalizedName + rom.Extension);

            if (!string.Equals(rom.FilePath, newPath, StringComparison.OrdinalIgnoreCase))
            {
                File.Move(rom.FilePath, newPath);
                rom.FilePath = newPath;
                rom.FileName = Path.GetFileName(newPath);
                rom.IsNormalized = true;
            }
        }
    }
}