using Microsoft.EntityFrameworkCore;
using TrueCode.UserService.Users;

namespace TrueCode.UserService.Authentication;

public class UserAuthenticationService : IAuthenticationService<User, TryGetResult<string>>
{
    private readonly UserContext _context;
    private readonly IAccessTokenCreator<UserDb> _accessTokenCreator;

    public UserAuthenticationService(UserContext context, IAccessTokenCreator<UserDb> accessTokenCreator)
    {
        _context = context;
        _accessTokenCreator = accessTokenCreator;
    }

    public async Task<TryGetResult<string>> TryGetTokenAsync(User userName)
    {
        var lfUser = await _context.Users.Where(x => x.Name == userName.Name).FirstOrDefaultAsync();

        if (lfUser == null)
        {
            return TryGetResult<string>.Failed();
        }

        return TryGetResult<string>.Success(_accessTokenCreator.CreateToken(lfUser));
    }
}