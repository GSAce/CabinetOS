using System.Security.Cryptography;

namespace CabinetOS.Core.Utilities
{
    public static class CRC32
    {
        private static readonly uint[] Table = Enumerable.Range(0, 256).Select(i =>
        {
            uint crc = (uint)i;
            for (int j = 0; j < 8; j++)
                crc = (crc & 1) != 0 ? (0xEDB88320 ^ (crc >> 1)) : (crc >> 1);
            return crc;
        }).ToArray();

        public static string Compute(Stream stream)
        {
            uint crc = 0xFFFFFFFF;

            int b;
            while ((b = stream.ReadByte()) != -1)
            {
                crc = Table[(crc ^ (byte)b) & 0xFF] ^ (crc >> 8);
            }

            crc ^= 0xFFFFFFFF;

            return crc.ToString("x8");
        }
    }
}