using Microsoft.EntityFrameworkCore;

namespace TrueCode.UserService.Users;

public class DatabaseUserService : IUserService
{
    private readonly UserContext _users;
    private readonly IHashCreator<User> _hashCreator;

    public DatabaseUserService(UserContext users,
        IHashCreator<User> hashCreator)
    {
        _users = users;

        _hashCreator = hashCreator;
    }

    public async Task CreateUserAsync(User user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user.Password, nameof(user.Password));

        var dbUser = new UserDb
        {
            Name = user.Name,
            Hash = _hashCreator.CreateHash(user)
        };

        try
        {
            await _users.Users.AddAsync(dbUser);
            await _users.SaveChangesAsync();
        }

        catch (DbUpdateConcurrencyException e)
        {
            throw new UserAlreadyExistsException() { Name = user.Name };
        }
        catch (DbUpdateException)
        {
            //TODO check it
            throw new UserAlreadyExistsException() { Name = user.Name };
        }
    }
}