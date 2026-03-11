using System.Diagnostics;
using System.Text.Json;

namespace CabinetOS.Core.Platforms.Detection;

public class PlatformRegistry
{
    public IReadOnlyDictionary<string, PlatformDefinition> Platforms => _platforms;
    private readonly Dictionary<string, PlatformDefinition> _platforms;

    public bool LoadedFromJson { get; private set; }
    public bool UsedFallbackDefaults { get; private set; }
    public List<string> RepairLog { get; } = new();

    private readonly string _jsonPath;

    public PlatformRegistry(string jsonPath)
    {
        _jsonPath = jsonPath;

        // 1. Try loading JSON
        if (!TryLoadJson(jsonPath, out var loaded))
        {
            RepairLog.Add("platforms.json missing or invalid → using fallback defaults.");
            _platforms = LoadFallbackDefaults();
            UsedFallbackDefaults = true;
            return;
        }

        // 2. Validate + repair
        _platforms = ValidateAndRepair(loaded);

        // 3. If JSON loaded but empty → fallback
        if (_platforms.Count == 0)
        {
            RepairLog.Add("platforms.json contained zero valid platforms → using fallback defaults.");
            _platforms = LoadFallbackDefaults();
            UsedFallbackDefaults = true;
        }
    }

    // ------------------------------------------------------------
    // JSON LOADING
    // ------------------------------------------------------------
    private bool TryLoadJson(string path, out Dictionary<string, PlatformDefinition>? result)
    {
        result = null;

        try
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"ERROR: platforms.json not found at: {path}");
                return false;
            }

            Debug.WriteLine($"Loading platforms.json from: {path}");
            var json = File.ReadAllText(path);

            // ⭐ CRITICAL FIX: Case-insensitive JSON property matching
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            result = JsonSerializer.Deserialize<Dictionary<string, PlatformDefinition>>(json, options);

            if (result == null)
            {
                Debug.WriteLine("ERROR: platforms.json deserialized to null.");
                return false;
            }

            LoadedFromJson = true;
            Debug.WriteLine($"PlatformRegistry loaded {result.Count} platforms.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ERROR loading PlatformRegistry: {ex.Message}");
            return false;
        }
    }

    // ------------------------------------------------------------
    // VALIDATION + AUTO-REPAIR
    // ------------------------------------------------------------
    private Dictionary<string, PlatformDefinition> ValidateAndRepair(Dictionary<string, PlatformDefinition> input)
    {
        var output = new Dictionary<string, PlatformDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var kvp in input)
        {
            var key = kvp.Key;
            var def = kvp.Value;

            if (def == null)
            {
                RepairLog.Add($"Platform '{key}' was null → skipped.");
                continue;
            }

            bool repaired = false;

            // ID must exist
            if (string.IsNullOrWhiteSpace(def.Id))
            {
                def.Id = key;
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing Id → auto-filled with '{key}'.");
            }

            // ShortName must exist
            if (string.IsNullOrWhiteSpace(def.ShortName))
            {
                def.ShortName = def.Id;
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing ShortName → auto-filled with '{def.Id}'.");
            }

            // Name must exist
            if (string.IsNullOrWhiteSpace(def.Name))
            {
                def.Name = def.ShortName;
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing Name → auto-filled with '{def.ShortName}'.");
            }

            // Category must exist
            if (string.IsNullOrWhiteSpace(def.Category))
            {
                def.Category = "Unknown";
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing Category → set to 'Unknown'.");
            }

            // FolderNames must exist
            if (def.FolderNames == null || def.FolderNames.Count == 0)
            {
                def.FolderNames = new List<string> { def.Id };
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing FolderNames → defaulted to ['{def.Id}'].");
            }

            // Extensions must exist
            if (def.Extensions == null || def.Extensions.Count == 0)
            {
                def.Extensions = new List<string> { "zip" };
                repaired = true;
                RepairLog.Add($"Platform '{key}' missing Extensions → defaulted to ['zip'].");
            }

            output[key] = def;

            if (repaired)
                Debug.WriteLine($"Repaired platform '{key}'.");
        }

        return output;
    }

    // ------------------------------------------------------------
    // FALLBACK DEFAULTS (TEMP ENUM)
    // ------------------------------------------------------------
    private Dictionary<string, PlatformDefinition> LoadFallbackDefaults()
    {
        var dict = new Dictionary<string, PlatformDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var p in TempPlatformDefaults.All)
            dict[p.Key] = p.Value;

        return dict;
    }

    // ------------------------------------------------------------
    // DEBUG OUTPUT
    // ------------------------------------------------------------
    public void DumpDebugInfo()
    {
        Debug.WriteLine("=== PlatformRegistry Debug Info ===");

        Debug.WriteLine($"LoadedFromJson: {LoadedFromJson}");
        Debug.WriteLine($"UsedFallbackDefaults: {UsedFallbackDefaults}");
        Debug.WriteLine($"Platform Count: {_platforms.Count}");

        if (RepairLog.Count > 0)
        {
            Debug.WriteLine("Repairs:");
            foreach (var entry in RepairLog)
                Debug.WriteLine($"  - {entry}");
        }
        else
        {
            Debug.WriteLine("Repairs: None");
        }

        Debug.WriteLine("Loaded Platforms:");
        foreach (var p in _platforms.Values)
            Debug.WriteLine($"  - {p.Id} ({p.Name})");

        Debug.WriteLine("===================================");
    }
}