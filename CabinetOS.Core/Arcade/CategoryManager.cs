namespace CabinetOS.Core.Arcade
{
    public static class CategoryManager
    {
        private static readonly Dictionary<string, List<string>> _categories =
            new(StringComparer.OrdinalIgnoreCase);

        private static bool _loaded = false;

        public static void EnsureLoaded()
        {
            if (_loaded)
                return;

            string folder = Path.Combine(AppContext.BaseDirectory, "Arcade", "Categories");

            if (!Directory.Exists(folder))
                return;

            foreach (var file in Directory.GetFiles(folder, "*.ini"))
            {
                LoadIni(file);
            }

            _loaded = true;
        }

        private static void LoadIni(string path)
        {
            string currentGame = "";
            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentGame = line.Trim('[', ']');
                    continue;
                }

                if (!line.Contains("="))
                    continue;

                var parts = line.Split('=', 2);
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (!_categories.TryGetValue(currentGame, out var list))
                {
                    list = new List<string>();
                    _categories[currentGame] = list;
                }

                list.Add($"{key}:{value}");
            }
        }

        public static List<string> GetCategories(string gameName)
        {
            EnsureLoaded();

            if (_categories.TryGetValue(gameName, out var list))
                return list;

            return new List<string>();
        }
    }
}