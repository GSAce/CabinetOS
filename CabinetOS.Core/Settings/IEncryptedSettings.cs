namespace CabinetOS.Core.Settings
{
    public interface IEncryptedSettings
    {
        void EncryptFields(string key);
        void DecryptFields(string key);
    }
}