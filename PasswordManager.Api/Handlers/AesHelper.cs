using System.Security.Cryptography;

namespace PasswordManager.Api.Handlers
{
    public class AesHelper
    {
        public static string EncryptAES(string text, byte[] key)
        {
            if(text == null || text.Length == 0)
                throw new ArgumentNullException("text");
            if(key == null || key.Length == 0)
                throw new ArgumentNullException("key");
            byte[] encryptedData;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memEncrypt = new MemoryStream())
                {
                    using(CryptoStream cEncrypt = new CryptoStream(memEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter sEncrypt = new StreamWriter(cEncrypt))
                        {
                            sEncrypt.Write(text);
                        }
                        encryptedData = memEncrypt.ToArray();
                    }
                    encryptedData = encryptedData.Concat(aes.IV).ToArray();
                }
            }
            return Convert.ToBase64String(encryptedData);
        }


        private static byte[] GetEncryptedPart(byte[] byteArray)
        {
            byte[] temp = new byte[byteArray.Length - 16];
            Array.Copy(byteArray, temp, byteArray.Length - 16);
            return temp;
        }

        private static byte[] GetIvPart(byte[] byteArray)
        {
            byte[] temp = new byte[16];
            Array.Copy(byteArray, temp.Length - 16, temp, 0, 16);
            return temp;
        }

        public static string DecryptAES(string cipherText, byte[] Key)
        {
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var cipherDataPart = GetEncryptedPart(cipherTextBytes);
            byte[] IV = GetIvPart(cipherTextBytes);

            if (cipherTextBytes == null || cipherTextBytes.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");


            string plaintext = null;


            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherDataPart))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
