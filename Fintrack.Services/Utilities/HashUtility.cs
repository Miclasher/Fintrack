using System.Security.Cryptography;

namespace Fintrack.Services.Utilities;

public static class HashUtility
{
    public static string HashPassword(string plainPassword, byte[] saltBytes)
    {
        return Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(plainPassword, saltBytes, 10000, HashAlgorithmName.SHA256, 32));
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword, string salt)
    {
        var plainHashed = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(plainPassword, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256, 32));

        return plainHashed == hashedPassword;
    }
}
