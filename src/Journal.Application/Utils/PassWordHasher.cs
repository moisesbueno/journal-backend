
namespace Journal.Application.Utils;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
    }

    public static bool VerifyPassword(string storedHash, string enteredPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(storedHash, enteredPassword);
    }
}
