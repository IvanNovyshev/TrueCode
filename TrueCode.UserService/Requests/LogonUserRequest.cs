namespace TrueCode.UserService.Requests;

public class LogonUserRequest
{
    public string Password { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}