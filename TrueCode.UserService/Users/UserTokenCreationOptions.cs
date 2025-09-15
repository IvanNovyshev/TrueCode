namespace TrueCode.UserService.Users;

public class UserTokenCreationOptions
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }

    public TimeSpan ExpiredIn { get; init; }
}