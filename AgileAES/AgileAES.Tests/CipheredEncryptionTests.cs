using AgileAES.Extensions;
using AgileAES.Models;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AgileAES.Tests
{
    [Parallelizable(ParallelScope.All)]
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
        public async Task CipheredEncryption_EncryptsAndDecrypts(string input)
        {
            var encrypted = await input.ToEncryptedSecureString();
            var decrypted = await encrypted.ToDecryptedSecureString();
            Assert.Greater(encrypted.String.Length, 0);
            Assert.AreEqual(input, decrypted.ToClearText());
        }

        [Test]
        public async Task CipheredEncryption_EncryptsAndDecrypts_WithCustomKeyAndIv()
        {
            string exampleKey = null;
            string exampleIv = null;

            using (var aes = Aes.Create())
            {
                exampleKey = Convert.ToBase64String(aes.Key);
                exampleIv = Convert.ToBase64String(aes.IV);
            }

            var pwd = "password";
            var encrypted = await pwd.ToEncryptedSecureString(exampleKey, exampleIv);
            var keyBytes = Convert.FromBase64String(exampleKey);
            var ivBytes = Convert.FromBase64String(exampleIv);
            Assert.NotNull(encrypted);
            Assert.NotNull(encrypted.String);
            Assert.AreNotEqual(pwd, encrypted.String.ToClearText());
            Assert.AreEqual(keyBytes, encrypted.Key);
            Assert.AreEqual(ivBytes, encrypted.IV);

            var clearEncryptedCipher = encrypted.String.ToClearText();
            var encryptedCipher = clearEncryptedCipher.ToSecureString();
            var secureStr = new EncryptedSecureString(encryptedCipher, key: keyBytes, iv: ivBytes);
            var decrypted = await secureStr.ToDecryptedSecureString();
            Assert.NotNull(decrypted);
            Assert.NotNull(decrypted.ToClearText());
            Assert.AreEqual(pwd, decrypted.ToClearText());
        }

        [TestCase("a", "")]
        [TestCase("a", " ")]
        [TestCase("", "a")]
        [TestCase(" ", "a")]
        public void CipheredEncryption_EncryptsAndDecrypts_InvalidKeyOrIv_ThrowsArgumentException(string key, string iv)
        {
            var task = "password".ToEncryptedSecureString(key, iv);
            Assert.ThrowsAsync<ArgumentException>(async () => await task);
        }
    }
}