
using System;
using System.Security.Cryptography;
using System.Text;

namespace SagErpBlazor.Data
{



    public static class EncryptionHelper
    {
        public static string DefaultKey = "CorbisKey";
        public static string DefaultEncrypt(string source, string key)
        {
            if (!string.IsNullOrEmpty(source))
            {
                using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
                {
                    using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                    {
                        byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                        tripleDESCryptoService.Key = byteHash;
                        tripleDESCryptoService.Mode = CipherMode.ECB;
                        byte[] data = Encoding.Unicode.GetBytes(source);
                        return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                    }
                }
            }
            else
            {
                return "";
            }
        }

        public static string DefaultDecrypt(string encrypt, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] byteBuff = Convert.FromBase64String(encrypt);
                    return Encoding.Unicode.GetString(tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                }
            }
        }
    }

}