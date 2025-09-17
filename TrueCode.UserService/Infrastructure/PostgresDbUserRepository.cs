using LinqToDB;
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
        // atomic insert with check
        var id = await _context.Users.Where(x => !_context.Users.Any(db => db.Name == user.Name))
            .InsertWithInt32IdentityAsync(
                _context.Users, db => new UserDb()
                    { Name = user.Name, Hash = user.Hash });

        if (id == null)
        {
            throw new UserAlreadyExistsException { Name = user.Name };
        }

        return new User { Id = id.Value, Name = user.Name };
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