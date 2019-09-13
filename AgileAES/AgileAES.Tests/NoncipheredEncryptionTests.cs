using AgileAES.Extensions;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AgileAES.Tests
{
    internal class NoncipheredEncryptionTests
    {
        [TestCase(" ")]
        [TestCase("a")]
        [TestCase(" a")]
        [TestCase("a ")]
        [TestCase(" a ")]
        [TestCase("test password with spaces")]
        [TestCase("01")]
        [TestCase(@"~!@#$%^&*()_+`-={}|[]\;':"",./<>?")]
        public async Task EncryptsAndDecrypts(string input)
        {
            var secureStr = input.ToSecureString();

            using (var aes = Aes.Create())
            {
                var encrypted = await Adapter.Encrypt(secureStr, aes.Key, aes.IV);
                var decrypted = await Adapter.Decrypt(encrypted, aes.Key, aes.IV);
                Assert.AreEqual(input, decrypted.ToClearText());
            }
        }
    }
}