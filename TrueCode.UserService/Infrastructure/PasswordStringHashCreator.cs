using System.Security.Cryptography;
using TrueCode.UserService.Core;

namespace TrueCode.UserService.Infrastructure;

public class PasswordStringHashCreator : IHashCreator<string>
{
    public string CreateHash(string password)
    {
        Span<byte> salt = stackalloc byte[16];
        RandomNumberGenerator.Fill(salt);

        Span<byte> hashSpan = stackalloc byte[32];

        Rfc2898DeriveBytes.Pbkdf2(
            password,
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