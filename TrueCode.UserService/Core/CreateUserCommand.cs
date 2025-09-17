namespace TrueCode.UserService.Core;

public class CreateUserCommand
{
    public required string Password { get; init; }
    public required string Name { get; init; }
}