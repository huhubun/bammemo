using Bammemo.Service.Abstractions.Enums;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Bammemo.Service;

public class SecurityService(
    IOptions<BammemoOptions> bammemoOptions) : ISecurityService
{
    private const string AES_KEY_FILE_NAME = "bammemo_key";

    public void GenerateAesKeyToLocalStorage()
    {
        File.WriteAllText(
            Path.Combine(bammemoOptions.Value.StoragePath, AES_KEY_FILE_NAME),
            GenerateAesKey());
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = LoadAesKeyAsync();
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();

        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var buffer = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = LoadAesKeyAsync();

        aes.IV = [.. buffer.Take(aes.IV.Length)];

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(buffer, aes.IV.Length, buffer.Length - aes.IV.Length);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    public KeySource GetKeySource()
        => File.Exists(GetKeyFilePath()) ? KeySource.LocalStorage : KeySource.Options;

    private byte[] LoadAesKeyAsync()
    {
        var base64Key = GetKeySource() switch
        {
            KeySource.LocalStorage => File.ReadAllText(GetKeyFilePath()),
            KeySource.Options => bammemoOptions.Value.Key ?? throw new OptionsValidationException("Bammemo.Key", typeof(string), ["Not configured"]),
            _ => throw new NotSupportedException()
        };

        return Convert.FromBase64String(base64Key);
    }

    private string GetKeyFilePath()
        => Path.Combine(bammemoOptions.Value.StoragePath, AES_KEY_FILE_NAME);

    private static string GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        return Convert.ToBase64String(aes.Key);
    }
}
