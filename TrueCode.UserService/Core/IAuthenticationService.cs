namespace TrueCode.UserService.Core;

public interface IAuthenticationService<in TIn, TOut>
{
    Task<TOut> TryGetTokenAsync(TIn userName);
}