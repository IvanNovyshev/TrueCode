namespace TrueCode.UserService.Core;

public interface IUserRepository
{
    public Task<User?> GetUserByNameOrDefaultAsync(string name);
    public Task<User> AddUserAsync(NewUser user);
}