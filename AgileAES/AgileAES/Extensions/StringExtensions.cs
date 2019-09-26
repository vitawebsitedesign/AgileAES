using AgileAES.Models;
using System.Security;
using System.Threading.Tasks;

namespace AgileAES.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts string to SecureString
        /// </summary>
        /// <param name="str">the clear text to convert</param>
        /// <returns>a read-only SecureString</returns>
        public static SecureString ToSecureString(this string str)
        {
            var secureStr = new SecureString();
            foreach (var c in str.ToCharArray())
            {
                secureStr.AppendChar(c);
            }
            secureStr.MakeReadOnly();
            return secureStr;
        }

        /// <summary>
        /// Encrypt to a SecureString ciphered in base64
        /// </summary>
        /// <param name="str">the clear text to encrypt</param>
        /// <returns>an encrypted read-only SecureString thats been ciphered in base64</returns>
        public static async Task<EncryptedSecureString> ToEncryptedSecureString(this string str, string key = null, string iv = null)
        {
            return await str.ToSecureString().ToEncryptedSecureString(key: key, iv: iv);
        }
    }
}
