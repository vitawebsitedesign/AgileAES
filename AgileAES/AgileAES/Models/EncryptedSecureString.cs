using System;
using System.Security;

namespace AgileAES.Models
{
    public class EncryptedSecureString : IDisposable
    {
        /// <summary>
        /// Encrypted read-only SecureString thats ciphered in base64
        /// </summary>
        public SecureString String { get; }
        /// <summary>
        /// Secret key that was used to encrypt the SecureString
        /// </summary>
        public byte[] Key { get; }
        /// <summary>
        /// Initialization vector that was used to encrypt the SecureString
        /// </summary>
        public byte[] IV { get; }

        /// <summary>
        /// A SecureString with key & IV metadata
        /// </summary>
        /// <param name="str">encrypted string ciphered in base64</param>
        /// <param name="key">secret key that was used to encrypt the string</param>
        /// <param name="iv">initialization vector that was used to encrypt the string</param>
        public EncryptedSecureString(SecureString str, byte[] key, byte[] iv)
        {
            str.MakeReadOnly();
            String = str;
            Key = key;
            IV = iv;
        }

        ~EncryptedSecureString()
        {
            Dispose();
        }

        public void Dispose()
        {
            String.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
