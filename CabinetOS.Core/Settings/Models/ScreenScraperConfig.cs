using CabinetOS.Core.Settings.Encryption;

namespace CabinetOS.Core.Settings.Models
{
    public class ScreenScraperConfig : IEncryptedSettings
    {
        // Developer credentials
        public string DevId { get; set; } = "";
        public string DevPasswordEncrypted { get; set; } = "";
        public string DevPasswordPlain { get; set; } = "";

        // User login (optional)
        public string UserName { get; set; } = "";
        public string UserPasswordEncrypted { get; set; } = "";
        public string UserPasswordPlain { get; set; } = "";

        // API key
        public string ApiKeyEncrypted { get; set; } = "";
        public string ApiKeyPlain { get; set; } = "";

        public void EncryptFields(string key)
        {
            if (!string.IsNullOrEmpty(DevPasswordPlain))
                DevPasswordEncrypted = AesEncryption.Encrypt(DevPasswordPlain, key);

            if (!string.IsNullOrEmpty(UserPasswordPlain))
                UserPasswordEncrypted = AesEncryption.Encrypt(UserPasswordPlain, key);

            if (!string.IsNullOrEmpty(ApiKeyPlain))
                ApiKeyEncrypted = AesEncryption.Encrypt(ApiKeyPlain, key);
        }

        public void DecryptFields(string key)
        {
            if (!string.IsNullOrEmpty(DevPasswordEncrypted))
                DevPasswordPlain = AesEncryption.Decrypt(DevPasswordEncrypted, key);

            if (!string.IsNullOrEmpty(UserPasswordEncrypted))
                UserPasswordPlain = AesEncryption.Decrypt(UserPasswordEncrypted, key);

            if (!string.IsNullOrEmpty(ApiKeyEncrypted))
                ApiKeyPlain = AesEncryption.Decrypt(ApiKeyEncrypted, key);
        }
    }
}