using System.Text;

namespace Bammemo.Service.Helpers;

public static class IdHelper
{
    public static string GenerateIdAlphabet()
    {
        var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToList();
        var idAlphabet = new StringBuilder();

        var random = new Random();

        while (chars.Count != 0)
        {
            var index = random.Next(chars.Count);

            idAlphabet.Append(chars[index]);
            chars.RemoveAt(index);
        }

        return idAlphabet.ToString();
    }
}
