namespace TrueCode.UserService.Authentication;

public interface IAuthenticationService<in TIn, TOut>
{
    Task<TOut> TryGetTokenAsync(TIn userName);
}