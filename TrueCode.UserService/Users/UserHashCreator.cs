using System.Security.Cryptography;

namespace TrueCode.UserService.Users;

public class UserHashCreator : IHashCreator<User>
{
    public string CreateHash(User user)
    {
        Span<byte> salt = stackalloc byte[16];
        RandomNumberGenerator.Fill(salt);

        Span<byte> hashSpan = stackalloc byte[32];

        Rfc2898DeriveBytes.Pbkdf2(
            user.Password,
            salt,
            hashSpan,
            100_000,
            HashAlgorithmName.SHA256
        );

        var result = new byte[16 + 32]; // layout: salt_hash
        salt.CopyTo(result);
        hashSpan.CopyTo(result.AsSpan(16));

        return Convert.ToBase64String(result);
    }
}