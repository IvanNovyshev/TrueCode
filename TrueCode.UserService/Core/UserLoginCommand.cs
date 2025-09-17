namespace TrueCode.UserService.Core;

public class UserLoginCommand
{
    public required string Name { get; init; }
    public required string Password { get; init; }
}