using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ExtensionsLibrary
{
    public static class StringEncryptionExtensions
    {
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/d6a2836a-d587-4068-8630-94f4fb2a2aeb/encrypt-and-decrypt-a-string-in-c?forum=csharpgeneral

        static readonly string PasswordHash = "^(Rk!~]3M";
        static readonly string SaltKey = ")8/Wq:@3";
        static readonly string VIKey = "*jm3@&M&q!k9i3~`";
        static readonly byte[] VIKeyBytes = Encoding.ASCII.GetBytes(VIKey);
        static byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);


        public static string Encrypt(this string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] cipherTextBytes;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros })
            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, VIKeyBytes))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }


        public static string Decrypt(this string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            int decryptedByteCount = 0;
            byte[] plainTextBytes;
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, VIKeyBytes))
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                plainTextBytes = new byte[cipherTextBytes.Length];
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
            }
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    }
}
