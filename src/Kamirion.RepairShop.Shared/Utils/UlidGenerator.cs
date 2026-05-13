using System.Security.Cryptography;

namespace Kamirion.RepairShop.Shared.Utils;

public static class UlidGenerator
{
    private static readonly char[] CrockfordBase32 = "0123456789ABCDEFGHJKMNPQRSTVWXYZ".ToCharArray();

    public static string New()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var random = new byte[10];
        RandomNumberGenerator.Fill(random);

        var chars = new char[26];

        // Timestamp: 10 chars × 5 bits = 50 bits (48 bits used)
        chars[0] = CrockfordBase32[(int)((timestamp >> 45) & 0x1F)];
        chars[1] = CrockfordBase32[(int)((timestamp >> 40) & 0x1F)];
        chars[2] = CrockfordBase32[(int)((timestamp >> 35) & 0x1F)];
        chars[3] = CrockfordBase32[(int)((timestamp >> 30) & 0x1F)];
        chars[4] = CrockfordBase32[(int)((timestamp >> 25) & 0x1F)];
        chars[5] = CrockfordBase32[(int)((timestamp >> 20) & 0x1F)];
        chars[6] = CrockfordBase32[(int)((timestamp >> 15) & 0x1F)];
        chars[7] = CrockfordBase32[(int)((timestamp >> 10) & 0x1F)];
        chars[8] = CrockfordBase32[(int)((timestamp >> 5) & 0x1F)];
        chars[9] = CrockfordBase32[(int)(timestamp & 0x1F)];

        // Randomness: 16 chars × 5 bits = 80 bits (10 bytes)
        chars[10] = CrockfordBase32[random[0] >> 3];
        chars[11] = CrockfordBase32[((random[0] & 0x07) << 2) | (random[1] >> 6)];
        chars[12] = CrockfordBase32[(random[1] >> 1) & 0x1F];
        chars[13] = CrockfordBase32[((random[1] & 0x01) << 4) | (random[2] >> 4)];
        chars[14] = CrockfordBase32[((random[2] & 0x0F) << 1) | (random[3] >> 7)];
        chars[15] = CrockfordBase32[(random[3] >> 2) & 0x1F];
        chars[16] = CrockfordBase32[((random[3] & 0x03) << 3) | (random[4] >> 5)];
        chars[17] = CrockfordBase32[random[4] & 0x1F];
        chars[18] = CrockfordBase32[random[5] >> 3];
        chars[19] = CrockfordBase32[((random[5] & 0x07) << 2) | (random[6] >> 6)];
        chars[20] = CrockfordBase32[(random[6] >> 1) & 0x1F];
        chars[21] = CrockfordBase32[((random[6] & 0x01) << 4) | (random[7] >> 4)];
        chars[22] = CrockfordBase32[((random[7] & 0x0F) << 1) | (random[8] >> 7)];
        chars[23] = CrockfordBase32[(random[8] >> 2) & 0x1F];
        chars[24] = CrockfordBase32[((random[8] & 0x03) << 3) | (random[9] >> 5)];
        chars[25] = CrockfordBase32[random[9] & 0x1F];

        return new string(chars);
    }
}
