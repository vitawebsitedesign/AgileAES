using AgileAES.Models;
using System;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AgileAES.Extensions
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Encrypts a SecureString with AES & base64 cipher
        /// </summary>
        /// <param name="str">the SecureString to encrypt</param>
        /// <returns>an AES encrypted read-only SecureString thats been ciphered in base64</returns>
        public static async Task<EncryptedSecureString> ToEncryptedSecureString(this SecureString str, string key = null, string iv = null)
        {
            if (key != null && key.Trim().Length == 0)
                throw new ArgumentException(nameof(key));
            if (iv != null && iv.Trim().Length == 0)
                throw new ArgumentException(nameof(iv));

            if (!str.IsReadOnly())
            {
                str.MakeReadOnly();
            }

            using (var aes = Aes.Create())
            {
                aes.Key = string.IsNullOrWhiteSpace(key) ? aes.Key : Convert.FromBase64String(key);
                aes.IV = string.IsNullOrWhiteSpace(iv) ? aes.IV : Convert.FromBase64String(iv);
                var encrypted = await Adapter.Encrypt(str, aes.Key, aes.IV);
                var ciphered = Convert.ToBase64String(encrypted);
                return new EncryptedSecureString(ciphered.ToSecureString(), aes.Key, aes.IV);
            }
        }

        /// <summary>
        /// Converts a SecureString into a clear text in-memory System.String
        /// </summary>
        /// <param name="secureStr">the SecureString to convert</param>
        /// <returns>a clear text in-memory string</returns>
        public static string ToClearText(this SecureString secureStr)
        {
            var plainEncryptedCipher = new NetworkCredential("", secureStr).Password;
            return plainEncryptedCipher;
        }
    }
}
