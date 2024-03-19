using System.Security.Cryptography;
using System.Text;

namespace MinimalTranslator.Application.Extensions;

public static class StringExtensions
{
    public static Guid GetHashedId(this string text)
    {
        byte[] hashBytes = SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(text))
            .Take(16)
            .ToArray();

        return new Guid(hashBytes);
    }
}