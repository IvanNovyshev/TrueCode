namespace TrueCode.UserService.Core;

public class CreateUserException : Exception
{
    public CreateUserException(string message, Exception? exception = null)
    {
    }
}