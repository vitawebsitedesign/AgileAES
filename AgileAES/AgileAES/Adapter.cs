using SecureStringCharacters;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgileAES
{
    public static class Adapter
    {
        /// <summary>
        /// Encrypts a SecureString
        /// </summary>
        /// <param name="str">the SecureString to encrypt</param>
        /// <param name="key">a secret key to encrypt with</param>
        /// <param name="iv">an initialization vector to encrypt with</param>
        /// <returns>encrypted byte array</returns>
        public static async Task<byte[]> Encrypt(SecureString str, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var ptr = Marshal.SecureStringToBSTR(str);

                using (var ms = new MemoryStream())
                {
                    try
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var sw = new StreamWriter(cs, Encoding.ASCII))
                        {
                            for (var c = 0; c < str.Length; c++)
                            {
                                var singleChar = (char)str.GetChar(c);
                                await sw.WriteAsync(singleChar);
                            }
                        }
                        return ms.ToArray();
                    }
                    finally
                    {
                        Marshal.ZeroFreeBSTR(ptr);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a byte array into a SecureString
        /// </summary>
        /// <param name="encrypted">the byte array to decrypt</param>
        /// <param name="key">the secret key that the byte array was encrypted with</param>
        /// <param name="iv">the initialization vector that the byte array was encrypted with</param>
        /// <returns></returns>
        public static async Task<SecureString> Decrypt(byte[] encrypted, byte[] key, byte[] iv)
        {
            var secureStr = new SecureString();
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream(encrypted))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.ASCII))
                {
                    while (sr.Peek() >= 0)
                    {
                        var chars = new char[1];
                        await sr.ReadAsync(chars, 0, 1);
                        secureStr.AppendChar(chars[0]);
                    }
                }
            }

            secureStr.MakeReadOnly();
            return secureStr;
        }
    }
}
