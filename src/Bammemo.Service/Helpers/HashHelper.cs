using System.Security.Cryptography;

namespace Bammemo.Service.Helpers;

public static class HashHelper
{
    public static (string algorithm, string hash) Sha256(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        using var sha256 = SHA256.Create();

        var hash = sha256.ComputeHash(stream);
        return (nameof(SHA256), Convert.ToHexStringLower(hash));
    }
}
