
namespace Journal.Application.Utils;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12);
    }

    public static bool VerifyPassword(string textPassword, string hashPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(textPassword, hashPassword);
    }
}
