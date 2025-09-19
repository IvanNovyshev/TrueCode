namespace TrueCode.UserService.Core;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message, Exception? exception = null)
    {
    }

    public required string Name { get; init; }
}