namespace BDRDExce.Commons;

using System.Security.Cryptography;
using System.Text;
using BDRDExce.Models;

public static class Utils {
    private static string[] _allowedContentTypes = {"image/jpeg", "image/png", "application/x-zip-compressed", "application/x-rar-compressed"};
    private const int _maxFileSize = 10 * 1024 * 1024;
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
    public static async Task<Media> ProcessUploadedFile(IFormFile file, HttpRequest request)
    {
        if(file.Length > 0 && file.Length <= _maxFileSize && _allowedContentTypes.Contains(file.ContentType))
        {
            using(var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                var id = Guid.NewGuid().ToString();
                var media = new Media
                {
                    Id = id,
                    ContentType = file.ContentType,
                    ContentName = file.FileName,
                    Content = fileBytes,
                    FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                };
                return media;
            }
        }
        return null;
    }
}                      
