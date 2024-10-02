using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BDRDExce.Commons.Utils;

public static class Utils {

    public static string Decrypt(string cipherText, string key)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = GetAesKey(key);
            byte[] iv     = new byte[aes.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];
 
            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
 
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new MemoryStream(cipher);
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
 
        private static byte[] GetAesKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] aesKey = new byte[32]; // For AES-256
            Array.Copy(keyBytes, aesKey, Math.Min(keyBytes.Length, aesKey.Length));
            return aesKey;
        }
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
}                      
