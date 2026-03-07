using System;
using System.IO;
using System.Text.Json;
using CabinetOS.Core.Settings.Encryption;

namespace CabinetOS.Core.Settings
{
    public static class SettingsService
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T Load<T>(string path, string encryptionKey = null) where T : new()
        {
            try
            {
                if (!File.Exists(path))
                    return new T();

                var json = File.ReadAllText(path);
                var model = JsonSerializer.Deserialize<T>(json, Options);

                if (model is IEncryptedSettings encrypted && encryptionKey != null)
                    encrypted.DecryptFields(encryptionKey);

                return model ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static void Save<T>(string path, T model, string encryptionKey = null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            if (model is IEncryptedSettings encrypted && encryptionKey != null)
                encrypted.EncryptFields(encryptionKey);

            var json = JsonSerializer.Serialize(model, Options);
            File.WriteAllText(path, json);

            if (model is IEncryptedSettings decrypted && encryptionKey != null)
                decrypted.DecryptFields(encryptionKey);
        }
    }
}