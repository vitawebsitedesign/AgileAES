### What
This .NET Standard library allows you to easily do AES encryption and/or ciphering between clear text & SecureString

<TODO NUGET URL>

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

`ToEncryptedSecureString` & `ToDecryptedSecureString` are helper methods, which also cipher the encrypted/decrypted result via base64.

### Usage (no ciphering)
If you dont wanna cipher stuff & just want to quickly do AES encryption, or want more control over the encryption parameters, you can also use the logic directly:
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
Pull requests are welcome. Please ensure the test suite passes before opening a PR.
