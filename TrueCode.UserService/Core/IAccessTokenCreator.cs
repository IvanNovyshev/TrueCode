namespace TrueCode.UserService.Core;

public interface IAccessTokenCreator<in T>
{
    public string CreateToken(T arg);
}