using AgileAES.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AgileAES.Tests
{
    internal class CipheredEncryptionTests
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
            var encrypted = await input.ToEncryptedSecureString();
            var decrypted = await encrypted.ToDecryptedSecureString();
            Assert.Greater(encrypted.String.Length, 0);
            Assert.AreEqual(input, decrypted.ToClearText());
        }
    }
}