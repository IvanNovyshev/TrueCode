namespace TrueCode.UserService.Users;

public interface IUserService
{
    Task CreateUserAsync(User user);
}