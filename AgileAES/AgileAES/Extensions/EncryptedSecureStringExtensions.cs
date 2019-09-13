using AgileAES.Models;
using System;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AgileAES.Extensions
{
    public static class EncryptedSecureStringExtensions
    {
        /// <summary>
        /// Decrypts an encrypted SecureString thats been ciphered in base64
        /// </summary>
        /// <param name="encryptedSecureStr">the encrypted SecureString thats been ciphered in base64</param>
        /// <returns>a decrypted non-ciphered read-only SecureString</returns>
        public static async Task<SecureString> ToDecryptedSecureString(this EncryptedSecureString encryptedSecureStr)
        {
            var ciphered = new NetworkCredential("", encryptedSecureStr.String).Password;
            var encrypted = Convert.FromBase64String(ciphered);
            using (var aes = Aes.Create())
            {
                aes.Key = encryptedSecureStr.Key;
                aes.IV = encryptedSecureStr.IV;
                return await Adapter.Decrypt(encrypted, aes.Key, aes.IV);
            }
        }
    }
}
