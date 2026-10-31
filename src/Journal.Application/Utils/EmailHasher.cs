using System.Security.Cryptography;
using System.Text;

namespace Journal.Application.Utils;

public static class EmailHasher
{
    // Deterministic pepper: required so emails can still be looked up by equality.
    // Changing this invalidates every stored email and breaks login for existing users.
    private const string Pepper = "dc7ca8167430963bd5f4157bbdb12f72f5db369c219e1c597b5d3ce09423eae0";

    public static string Hash(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Pepper));

        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(normalized))).ToLowerInvariant();
    }
}
