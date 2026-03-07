using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CabinetOS.Core.Arcade.RomScanner.Hashing
{
    public static class HashingHelpers
    {
        public static HashCacheEntry GetOrComputeHash(string filePath, HashCache cache)
        {
            var info = new FileInfo(filePath);

            if (cache.Entries.TryGetValue(filePath, out var entry) &&
                entry.Size == info.Length &&
                entry.LastModifiedUtc == info.LastWriteTimeUtc)
            {
                return entry; // Cache hit
            }

            var newEntry = new HashCacheEntry
            {
                Path = filePath,
                Size = info.Length,
                LastModifiedUtc = info.LastWriteTimeUtc,
                SHA1 = ComputeSha1(filePath),
                MD5 = ComputeMd5(filePath),
                CRC = ComputeCrc(filePath)
            };

            cache.Entries[filePath] = newEntry;
            return newEntry;
        }

        public static string ComputeSha1(string path)
        {
            using var sha1 = SHA1.Create();
            using var stream = File.OpenRead(path);
            return ToHex(sha1.ComputeHash(stream));
        }

        public static string ComputeMd5(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);
            return ToHex(md5.ComputeHash(stream));
        }

        public static string ComputeCrc(string path)
        {
            using var stream = File.OpenRead(path);
            using var crc32 = new Crc32();
            return ToHex(crc32.ComputeHash(stream));
        }

        private static string ToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        // ------------------------------------------------------------
        // CRC32 implementation
        // ------------------------------------------------------------
        private sealed class Crc32 : HashAlgorithm
        {
            public const uint Polynomial = 0xEDB88320u;
            public const uint Seed = 0xFFFFFFFFu;

            private static readonly uint[] Table = InitializeTable();
            private uint _hash;

            public Crc32()
            {
                Initialize();
            }

            public override void Initialize()
            {
                _hash = Seed;
            }

            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                for (int i = ibStart; i < ibStart + cbSize; i++)
                {
                    unchecked
                    {
                        _hash = (_hash >> 8) ^ Table[array[i] ^ (_hash & 0xFF)];
                    }
                }
            }

            protected override byte[] HashFinal()
            {
                var final = ~_hash;
                return new[]
                {
                    (byte)((final >> 24) & 0xFF),
                    (byte)((final >> 16) & 0xFF),
                    (byte)((final >> 8) & 0xFF),
                    (byte)(final & 0xFF)
                };
            }

            public override int HashSize => 32;

            private static uint[] InitializeTable()
            {
                var table = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    uint entry = i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ Polynomial;
                        else
                            entry >>= 1;
                    }
                    table[i] = entry;
                }
                return table;
            }
        }
    }
}