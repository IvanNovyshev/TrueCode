namespace TrueCode.UserService.Core;

public class UserAuthenticationService : IAuthenticationService<UserLoginCommand, TryGetResult<string>>
{
    private readonly IUserRepository _repository;
    private readonly IAccessTokenCreator<User> _accessTokenCreator;

    public UserAuthenticationService(IAccessTokenCreator<User> accessTokenCreator, IUserRepository repository)
    {
        _accessTokenCreator = accessTokenCreator;
        _repository = repository;
    }

    public async Task<TryGetResult<string>> TryGetTokenAsync(UserLoginCommand command)
    {
        try
        {
            var lfUser = await _repository.GetUserByNameOrDefaultAsync(command.Name);

            if (lfUser == null)
            {
                return TryGetResult<string>.Failed();
            }

            return TryGetResult<string>.Success(_accessTokenCreator.CreateToken(lfUser));
        }
        catch (Exception e)
        {
            throw new UserAuthenticationException(e.Message, e);
        }
    }
}

public class UserAuthenticationException : Exception
{
    public UserAuthenticationException(string message, Exception? e = null) : base(message, e)
    {
    }
}