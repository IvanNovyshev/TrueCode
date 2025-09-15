namespace TrueCode.UserService.Users;

public interface IHashCreator<in T>
{
    public string CreateHash(T arg);
}