using LinqToDB;
using LinqToDB.DataProvider.PostgreSQL;
using Npgsql;
using TrueCode.UserService.Core;

namespace TrueCode.UserService.Infrastructure;

public class PostgresDbUserRepository : IUserRepository
{
    private readonly UserContext _context;

    public PostgresDbUserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByNameOrDefaultAsync(string name)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
        if (dbUser == null)
        {
            return null;
        }

        return MapToDomain(dbUser);
    }

    public async Task<User> AddUserAsync(NewUser user)
    {
        try
        {
            var id = await _context.Users.InsertWithInt32IdentityAsync(() => new UserDb
                { Name = user.Name, Hash = user.Hash });
            return new User { Id = id, Name = user.Name };
        }
        catch (PostgresException e) when (e.MessageText.Contains("duplicate"))
        {
            throw new UserAlreadyExistsException(e.Message, e) { Name = user.Name };
        }
    }


    private User MapToDomain(UserDb userDb)
    {
        return new User
        {
            Id = userDb.Id,
            Name = userDb.Name,
        };
    }
}