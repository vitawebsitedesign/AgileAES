using AgileAES.Extensions;
using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var encrypted = await "password".ToEncryptedSecureString();
            var decrypted = await encrypted.ToDecryptedSecureString();

            Console.WriteLine(SecureStringToString(encrypted.String));
            Console.WriteLine(SecureStringToString(decrypted));
            encrypted.Dispose();
            decrypted.Dispose();
            Console.ReadKey();
        }

        private static string SecureStringToString(SecureString secureStr)
        {
            var plainEncryptedCipher = new NetworkCredential("", secureStr).Password;
            return plainEncryptedCipher;
        }
    }
}
