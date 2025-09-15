namespace TrueCode.UserService.Authentication;

public interface IAccessTokenCreator<in T>
{
    public string CreateToken(T arg);
}