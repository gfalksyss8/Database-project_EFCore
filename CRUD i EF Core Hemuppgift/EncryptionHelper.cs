using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_i_EF_Core_Hemuppgift
{
    public class EncryptionHelper
    {
        private const byte Key = 0x42; // 66 bytes

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(text);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte) (bytes[i] ^ Key);
            }

            return Convert.ToBase64String(bytes);
        }

        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text; 
            }

            var bytes = Convert.FromBase64String(text);

            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte)(bytes[i] ^ Key);
            }

            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashWithSalt(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }

        }

        public static string HashWithSalt2(string value, string base64salt, int iterations = 100_000, int hashLength = 32)
        {
            var saltBytes = Convert.FromBase64String(base64salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(value, saltBytes, iterations, HashAlgorithmName.SHA256);

            var hashBytes = pbkdf2.GetBytes(hashLength);
            return Convert.ToBase64String(hashBytes);
        }

        public static string Verify2(string value, string base64Salt, string expectedBase64Hash)
        {
            var computedHash = HashWithSalt2(value, base64Salt);
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(computedHash),
                Convert.FromBase64String(expectedBase64Hash)
            ) ? computedHash : string.Empty;
        }
    }
}
