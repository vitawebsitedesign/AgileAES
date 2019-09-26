using AgileAES.Extensions;
using System;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var encrypted = await "password".ToEncryptedSecureString();
            var decrypted = await encrypted.ToDecryptedSecureString();

            Console.WriteLine($"encrypted: {encrypted.String.ToClearText()}");
            Console.WriteLine($"decrypted: {decrypted.ToClearText()}");
            encrypted.Dispose();
            decrypted.Dispose();
            Console.ReadKey();
        }
    }
}
