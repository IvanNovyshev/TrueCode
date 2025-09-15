using FluentValidation;
using TrueCode.UserService.Authentication;
using TrueCode.UserService.Requests;
using TrueCode.UserService.Requests.Validation;
using TrueCode.UserService.Users;

namespace TrueCode.UserService;

public static class Extensions
{
    public static IServiceCollection AddUserServicesDependencies(this IServiceCollection collection,
        IConfiguration config)
    {
      

        collection.Configure<UserTokenCreationOptions>(config.GetSection("Token"));
        collection.AddSingleton<IAccessTokenCreator<UserDb>, AccessTokenCreator>();
        collection.AddScoped<IAuthenticationService<User, TryGetResult<string>>, UserAuthenticationService>();


        collection.AddScoped<IUserService, DatabaseUserService>();
        collection.AddSingleton<IHashCreator<User>, UserHashCreator>();


        collection.AddSingleton<IValidator<LogonUserRequest>, LogonUserRequestValidator>();
        collection.AddSingleton<IValidator<CreateUserRequest>, CreateUserRequestValidator>();


        return collection;
    }
}