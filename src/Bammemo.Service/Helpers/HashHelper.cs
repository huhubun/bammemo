using Bammemo.Service.Enums;
using System.Security.Cryptography;

namespace Bammemo.Service.Helpers;

public static class HashHelper
{
    public static (string algorithm, string hash) Sha256(Stream stream, BinaryEncodingType type = BinaryEncodingType.Base64)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        using var sha256 = SHA256.Create();

        var hashByte = sha256.ComputeHash(stream);
        return (nameof(SHA256), GetEncodedString(hashByte, type));
    }

    public static (string algorithm, string hash) Sha384(string filePath, BinaryEncodingType type = BinaryEncodingType.Base64)
    {
        using var fileStream = File.OpenRead(filePath);
        return Sha384(fileStream, type);
    }

    public static (string algorithm, string hash) Sha384(Stream stream, BinaryEncodingType type = BinaryEncodingType.Base64)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        using var sha384 = SHA384.Create();

        var hashByte = sha384.ComputeHash(stream);
        return (nameof(SHA384), GetEncodedString(hashByte, type));
    }

    private static string GetEncodedString(byte[] bytes, BinaryEncodingType type) => type switch
    {
        BinaryEncodingType.Base64 => Convert.ToBase64String(bytes),
        BinaryEncodingType.Hex => Convert.ToHexStringLower(bytes),
        _ => throw new NotSupportedException(type.ToString())
    };
}
