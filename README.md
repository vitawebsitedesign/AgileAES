### What
This .NET Standard library allows you to easily do AES encryption and/or ciphering between clear text & SecureString.

The compiled Nuget package has been made available [on Nuget.org](https://www.nuget.org/packages/MichaelNguyen.AgileAES/)

### Install
```bash
Install-Package MichaelNguyen.AgileAES
```

### Usage
Import namespace...
```c#
using AgileAES.Extensions;
```

Encrypting:
```c#
var encrypted = await "password".ToEncryptedSecureString();
```

Decrypting:
```c#
var decrypted = await encrypted.ToDecryptedSecureString();
```

`ToEncryptedSecureString` & `ToDecryptedSecureString` are helper methods, which also cipher/decipher the result via base64.

This repo contains an "example usage" project.

Note: Realistically, encryption & decryption happen at different times. You may prefer to save a secret key & initialization vector to disk during encryption, so that you can retrieve them at a later stage to decrypt stuff. This is demonstrated in the next section.

### Usage (custom key & IV)
You can also encrypt/decrypt with a custom secret key or initialization vector:
``` c#
// Get a key and IV for demo purposes.
// Usually you already have this data from an API, database, or a local file, but for completeness I'll show how to generate some anyways
byte[] keyBytes = null;
byte[] ivBytes = null;
string key = null;
string iv = null;
using (var aes = Aes.Create())
{
	keyBytes = aes.Key;
	ivBytes = aes.IV;
	key = Convert.ToBase64String(keyBytes);
	iv = Convert.ToBase64String(ivBytes);
}

// Encrypt with custom secret key and IV
var encrypted = await "password".ToEncryptedSecureString(key, iv);

// Decrypt with custom secret key and IV
var secureStr = new EncryptedSecureString(encryptedCipher, key: key, iv: iv);
var decrypted = await secureStr.ToDecryptedSecureString();
````

### Usage (no ciphering)
Example of doing AES encyrption without base64 ciphering:
```c#
using (var aes = Aes.Create())
{
    var encrypted = await Adapter.Encrypt(str, aes.Key, aes.IV);
}
```
```c#
using (var aes = Aes.Create())
{
    aes.Key = //<the secret key that was used to encrypt>
    aes.IV = //<the initialization vector that was used to encrypt>
    return await Adapter.Decrypt(str, aes.Key, aes.IV);
}
```

### Contributing
Pull requests are welcome. Please ensure that new tests are thread-safe, & that the all test fixtures pass before opening a PR.
