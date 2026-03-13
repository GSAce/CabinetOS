using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CabinetOS.Core.Input
{
    public class ControllerMappingStore
    {
        public Dictionary<int, ControllerProfile> Controllers { get; set; }

        public ControllerMappingStore()
        {
            Controllers = new Dictionary<int, ControllerProfile>();
        }

        public void SetMapping(int controllerIndex, string action, string input, bool isAxis)
        {
            if (!Controllers.ContainsKey(controllerIndex))
                Controllers[controllerIndex] = new ControllerProfile();

            var profile = Controllers[controllerIndex];

            if (isAxis)
                profile.AxisBindings[action] = input;
            else
                profile.ButtonBindings[action] = input;

            Save();
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText("controller_mappings.json", json);
        }

        public static ControllerMappingStore Load()
        {
            if (!File.Exists("controller_mappings.json"))
                return new ControllerMappingStore();

            var json = File.ReadAllText("controller_mappings.json");
            return JsonSerializer.Deserialize<ControllerMappingStore>(json)
                   ?? new ControllerMappingStore();
        }
    }

    public class ControllerProfile
    {
        public Dictionary<string, string> ButtonBindings { get; set; }
        public Dictionary<string, string> AxisBindings { get; set; }

        public ControllerProfile()
        {
            ButtonBindings = new Dictionary<string, string>();
            AxisBindings = new Dictionary<string, string>();
        }
    }
}