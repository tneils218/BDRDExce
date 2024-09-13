using System.Security.Cryptography;
using System.Text;

namespace BDRDExce.Commons.Utils;

public static class Utils {
    public static string ComputeSha256Hash(string text)
    {
        UnicodeEncoding ue = new UnicodeEncoding();
        byte[] hashValue;
        //byte[] message = ue.GetBytes("NYRR" + text + "RRYN");
        byte[] message = ue.GetBytes(text.Length * 2 + text + text.Length * 2);

        SHA256Managed hashString = new SHA256Managed();
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        // Tạo mảng byte từ chuỗi thập lục phân
        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            // Chuyển đổi từng cặp ký tự hex thành byte
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        // Chuyển mảng byte thành chuỗi Unicode
        return Encoding.UTF8.GetString(bytes);
    }

    public static string Decrypt(string cipherText, string key)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
 
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetAesKey(key);
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];
 
                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
 
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
 
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
 
        private static byte[] GetAesKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] aesKey = new byte[32]; // For AES-256
            Array.Copy(keyBytes, aesKey, Math.Min(keyBytes.Length, aesKey.Length));
            return aesKey;
        }
}                      
