namespace TrueCode.UserService.Users;

public class UserAlreadyExistsException : Exception
{
    public required string Name { get; init; }
}