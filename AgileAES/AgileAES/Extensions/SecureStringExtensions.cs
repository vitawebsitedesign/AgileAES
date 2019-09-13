using AgileAES.Models;
using System;
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
        public static async Task<EncryptedSecureString> ToEncryptedSecureString(this SecureString str)
        {
            if (!str.IsReadOnly())
            {
                str.MakeReadOnly();
            }

            using (var aes = Aes.Create())
            {
                var encrypted = await Adapter.Encrypt(str, aes.Key, aes.IV);
                var ciphered = Convert.ToBase64String(encrypted);
                return new EncryptedSecureString(ciphered.ToSecureString(), aes.Key, aes.IV);
            }
        }
    }
}
