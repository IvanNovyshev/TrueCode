namespace TrueCode.UserService.Core;

public interface IUserService
{
    Task CreateUserAsync(CreateUserCommand user);
}