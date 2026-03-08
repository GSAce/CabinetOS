using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CabinetOS.Core.Settings.Encryption;
using CabinetOS.Core.Settings.Models;

namespace CabinetOS.Core.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string EncryptionKey = "CabinetOS-Default-Encryption-Key";

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public CabinetSettings Current { get; private set; } = new();

        private readonly string _settingsFile;
        private readonly string _secretsFile;

        public SettingsService()
        {
            _settingsFile = SettingsPaths.SettingsFile;
            _secretsFile = SettingsPaths.SecretsFile;

            Directory.CreateDirectory(SettingsPaths.Root);
        }

        public async Task LoadAsync()
        {
            // Load main settings
            if (File.Exists(_settingsFile))
            {
                var json = await File.ReadAllTextAsync(_settingsFile);
                var loaded = JsonSerializer.Deserialize<CabinetSettings>(json, _jsonOptions);

                if (loaded != null)
                    Current = loaded;
            }
            else
            {
                await SaveAsync(); // create defaults
            }

            // Load encrypted secrets
            if (File.Exists(_secretsFile))
            {
                var encryptedJson = await File.ReadAllTextAsync(_secretsFile);
                var encrypted = JsonSerializer.Deserialize<EncryptedSettingsBlob>(encryptedJson, _jsonOptions);

                if (encrypted != null)
                    ApplyDecryptedSecrets(encrypted);
            }
        }

        public async Task SaveAsync()
        {
            // Save unencrypted settings
            var json = JsonSerializer.Serialize(Current, _jsonOptions);
            await File.WriteAllTextAsync(_settingsFile, json);

            // Save encrypted secrets
            var encrypted = ExtractEncryptedSecrets();
            var encryptedJson = JsonSerializer.Serialize(encrypted, _jsonOptions);
            await File.WriteAllTextAsync(_secretsFile, encryptedJson);
        }

        private EncryptedSettingsBlob ExtractEncryptedSecrets()
        {
            // Let each model encrypt its own fields
            Current.Igdb.EncryptFields(EncryptionKey);
            Current.ScreenScraper.EncryptFields(EncryptionKey);

            return new EncryptedSettingsBlob
            {
                // IGDB
                IgdbClientId = Current.Igdb.ClientId,
                IgdbClientSecret = Current.Igdb.ClientSecretEncrypted,

                // ScreenScraper developer credentials
                ScreenScraperDevId = Current.ScreenScraper.DevId,
                ScreenScraperDevPassword = Current.ScreenScraper.DevPasswordEncrypted,

                // ScreenScraper user login
                ScreenScraperUserName = Current.ScreenScraper.UserName,
                ScreenScraperUserPassword = Current.ScreenScraper.UserPasswordEncrypted,

                // ScreenScraper API key
                ScreenScraperApiKey = Current.ScreenScraper.ApiKeyEncrypted,

                // TheGamesDb
                TheGamesDbApiKey = Current.TheGamesDb.ApiKey
            };
        }

        private void ApplyDecryptedSecrets(EncryptedSettingsBlob blob)
        {
            // IGDB
            Current.Igdb.ClientId = blob.IgdbClientId;
            Current.Igdb.ClientSecretEncrypted = blob.IgdbClientSecret;
            Current.Igdb.DecryptFields(EncryptionKey);

            // ScreenScraper developer credentials
            Current.ScreenScraper.DevId = blob.ScreenScraperDevId;
            Current.ScreenScraper.DevPasswordEncrypted = blob.ScreenScraperDevPassword;

            // ScreenScraper user login
            Current.ScreenScraper.UserName = blob.ScreenScraperUserName;
            Current.ScreenScraper.UserPasswordEncrypted = blob.ScreenScraperUserPassword;

            // ScreenScraper API key
            Current.ScreenScraper.ApiKeyEncrypted = blob.ScreenScraperApiKey;

            // Decrypt all ScreenScraper fields
            Current.ScreenScraper.DecryptFields(EncryptionKey);

            // TheGamesDb
            Current.TheGamesDb.ApiKey = blob.TheGamesDbApiKey;
        }
    }
}