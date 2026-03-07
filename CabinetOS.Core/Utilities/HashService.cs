using System.Security.Cryptography;

namespace CabinetOS.Core.Utilities
{
    public static class HashService
    {
        public static string ComputeMD5(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return ComputeMD5(stream);
        }

        public static string ComputeSHA1(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return ComputeSHA1(stream);
        }
        public static string ComputeSHA1ForFile(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(stream);
            return Convert.ToHexString(hash).ToLower();
        }
        public static string ComputeMD5(Stream stream)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return Convert.ToHexString(hash).ToLower();
        }

        public static string ComputeSHA1(Stream stream)
        {
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(stream);
            return Convert.ToHexString(hash).ToLower();
        }
    }
}