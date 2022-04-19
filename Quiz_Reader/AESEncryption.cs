using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Quiz_Generator
{
    class AESEncryption : IEncryption
    {
        private AesCryptoServiceProvider Aes;

        public AESEncryption()
        {
            Aes = new AesCryptoServiceProvider();
            Aes.BlockSize = 128;
            Aes.KeySize = 256;
            Aes.IV = Encoding.UTF8.GetBytes("A?D(G-KaPdSgVkYp");
            Aes.Mode = CipherMode.CBC;
        }

        public string EncryptString(string key, string plainText)
        {
            Aes.Key = Encoding.UTF8.GetBytes(key);
            var encryptor = Aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), Aes.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DecryptString(string key, string cipherText)
        {
            Aes.Key = Encoding.UTF8.GetBytes(key);
            var decryptor = Aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), Aes.IV);
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
