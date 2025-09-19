namespace TrueCode.UserService.Core;

public class RepositoryUserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IHashCreator<string> _hashCreator;

    public RepositoryUserService(IUserRepository repository,
        IHashCreator<string> hashCreator)
    {
        _repository = repository;
        _hashCreator = hashCreator;
    }

    public async Task CreateUserAsync(CreateUserCommand user)
    {
        try
        {
            await _repository.AddUserAsync(new NewUser
                { Name = user.Name, Hash = _hashCreator.CreateHash(user.Password) });
        }
        catch (UserAlreadyExistsException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CreateUserException(e.Message, e.InnerException);
        }
    }
}