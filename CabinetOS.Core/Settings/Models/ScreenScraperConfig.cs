using CabinetOS.Core.Settings.Encryption;

namespace CabinetOS.Core.Settings.Models
{
    public class ScreenScraperConfig : IEncryptedSettings
    {
        public string DevId { get; set; } = "";
        public string DevPasswordEncrypted { get; set; } = "";
        public string ApiKeyEncrypted { get; set; } = "";

        // Runtime-only decrypted fields
        public string DevPasswordPlain { get; set; } = "";
        public string ApiKeyPlain { get; set; } = "";

        public void EncryptFields(string key)
        {
            if (!string.IsNullOrEmpty(DevPasswordPlain))
                DevPasswordEncrypted = AesEncryption.Encrypt(DevPasswordPlain, key);

            if (!string.IsNullOrEmpty(ApiKeyPlain))
                ApiKeyEncrypted = AesEncryption.Encrypt(ApiKeyPlain, key);
        }

        public void DecryptFields(string key)
        {
            if (!string.IsNullOrEmpty(DevPasswordEncrypted))
                DevPasswordPlain = AesEncryption.Decrypt(DevPasswordEncrypted, key);

            if (!string.IsNullOrEmpty(ApiKeyEncrypted))
                ApiKeyPlain = AesEncryption.Decrypt(ApiKeyEncrypted, key);
        }
    }
}