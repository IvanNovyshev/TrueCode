namespace TrueCode.UserService.Requests;

public class CreateUserRequest
{
    public string Password { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}