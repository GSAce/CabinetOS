using CabinetOS.Core.Settings.Encryption;

namespace CabinetOS.Core.Settings.Models
{
    public class ScraperSettings : IEncryptedSettings
    {
        public string ApiUrl { get; set; } = "";
        public string Username { get; set; } = "";
        public string PasswordEncrypted { get; set; } = "";
        public bool EnableHighResImages { get; set; } = true;

        // Not serialized — decrypted at runtime
        public string PasswordPlain { get; set; } = "";

        public void EncryptFields(string key)
        {
            if (!string.IsNullOrEmpty(PasswordPlain))
                PasswordEncrypted = AesEncryption.Encrypt(PasswordPlain, key);
        }

        public void DecryptFields(string key)
        {
            if (!string.IsNullOrEmpty(PasswordEncrypted))
                PasswordPlain = AesEncryption.Decrypt(PasswordEncrypted, key);
        }
    }
}