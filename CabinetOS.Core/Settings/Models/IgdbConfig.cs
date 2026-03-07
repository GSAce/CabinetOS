using CabinetOS.Core.Settings.Encryption;

namespace CabinetOS.Core.Settings.Models
{
    public class IgdbConfig : IEncryptedSettings
    {
        public string ClientId { get; set; } = "";
        public string ClientSecretEncrypted { get; set; } = "";

        public string ClientSecretPlain { get; set; } = "";

        public void EncryptFields(string key)
        {
            if (!string.IsNullOrEmpty(ClientSecretPlain))
                ClientSecretEncrypted = AesEncryption.Encrypt(ClientSecretPlain, key);
        }

        public void DecryptFields(string key)
        {
            if (!string.IsNullOrEmpty(ClientSecretEncrypted))
                ClientSecretPlain = AesEncryption.Decrypt(ClientSecretEncrypted, key);
        }
    }
}