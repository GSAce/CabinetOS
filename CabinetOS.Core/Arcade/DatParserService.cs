using CabinetOS.Core.Arcade.Models;
using System.Xml.Linq;

namespace CabinetOS.Core.Arcade
{
    public static class DatParserService
    {
        public static List<DatGameInfo> LoadDat(string datFilePath)
        {
            if (!File.Exists(datFilePath))
                throw new FileNotFoundException("DAT file not found", datFilePath);

            var doc = XDocument.Load(datFilePath);
            var root = doc.Root;

            if (root == null)
                throw new InvalidDataException("Invalid DAT file: missing root element.");

            // MAME/Logiqx style: <datafile><game>...</game></datafile>
            var gameElements = root.Elements("game").ToList();
            if (!gameElements.Any())
            {
                // Some DATs use <machine> instead of <game>
                gameElements = root.Elements("machine").ToList();
            }

            List<DatGameInfo> games = new();

            foreach (var gameElem in gameElements)
            {
                var game = new DatGameInfo
                {
                    Name = (string?)gameElem.Attribute("name") ?? string.Empty,
                    Description = (string?)gameElem.Element("description") ?? string.Empty,
                    Parent = (string?)gameElem.Attribute("parent"),
                    CloneOf = (string?)gameElem.Attribute("cloneof"),
                    Year = (string?)gameElem.Element("year"),
                    Manufacturer = (string?)gameElem.Element("manufacturer"),

                    // Flags from attributes
                    IsMechanical = GetBool(gameElem.Attribute("ismechanical")),
                    IsPrototype = GetBool(gameElem.Attribute("isprototype")),
                    IsNonWorking = IsNonWorking(gameElem),

                    // Flags from categories
                    IsGambling = HasCategory(gameElem, "Gambling") || HasCategory(gameElem, "Casino"),
                    IsFruitMachine = HasCategory(gameElem, "Fruit") || HasCategory(gameElem, "Fruit Machines"),
                    IsTicketRedemption = HasCategory(gameElem, "Redemption"),
                    IsQuiz = HasCategory(gameElem, "Quiz"),
                    IsAdult = HasCategory(gameElem, "Adult"),
                    IsCasino = HasCategory(gameElem, "Casino"),
                    IsMahjong = HasCategory(gameElem, "Mahjong")
                };

                // ROMs
                foreach (var romElem in gameElem.Elements("rom"))
                {
                    var rom = new DatRomInfo
                    {
                        Name = (string?)romElem.Attribute("name") ?? string.Empty,
                        Size = ParseLong((string?)romElem.Attribute("size")),
                        CRC = (string?)romElem.Attribute("crc"),
                        MD5 = (string?)romElem.Attribute("md5"),
                        SHA1 = (string?)romElem.Attribute("sha1"),
                        IsBios = ((string?)romElem.Attribute("bios")) != null,
                        IsDevice = ((string?)romElem.Attribute("merge")) != null && ((string?)romElem.Attribute("merge"))!.Contains("device", StringComparison.OrdinalIgnoreCase),
                        IsSample = false // samples are usually separate entries
                    };

                    game.Roms.Add(rom);
                }

                // Disks (CHDs)
                foreach (var diskElem in gameElem.Elements("disk"))
                {
                    var disk = new DatDiskInfo
                    {
                        Name = (string?)diskElem.Attribute("name") ?? string.Empty,
                        MD5 = (string?)diskElem.Attribute("md5"),
                        SHA1 = (string?)diskElem.Attribute("sha1"),
                        Region = (string?)diskElem.Attribute("region")
                    };

                    game.Disks.Add(disk);
                }

                games.Add(game);
            }

            return games;
        }
        private static bool GetBool(XAttribute? attr)
        {
            if (attr == null) return false;
            return attr.Value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasCategory(XElement gameElem, string category)
        {
            var cat = (string?)gameElem.Element("category");
            if (cat == null) return false;
            return cat.Contains(category, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsNonWorking(XElement gameElem)
        {
            var driver = gameElem.Element("driver");
            if (driver == null) return false;

            string status = (string?)driver.Attribute("status") ?? "";
            return status.Equals("preliminary", StringComparison.OrdinalIgnoreCase)
                || status.Equals("imperfect", StringComparison.OrdinalIgnoreCase);
        }
        private static long ParseLong(string? value)
        {
            if (long.TryParse(value, out var result))
                return result;
            return 0;
        }
    }
}