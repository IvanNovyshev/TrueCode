namespace TrueCode.UserService.Core;

public class UserAlreadyExistsException : Exception
{
    public required string Name { get; init; }
}