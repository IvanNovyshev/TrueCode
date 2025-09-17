namespace TrueCode.UserService.Core;

public interface IHashCreator<in T>
{
    public string CreateHash(T arg);
}