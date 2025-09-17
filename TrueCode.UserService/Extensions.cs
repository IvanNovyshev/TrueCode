using FluentValidation;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using TrueCode.UserService.Core;
using TrueCode.UserService.Infrastructure;
using TrueCode.UserService.Requests;
using TrueCode.UserService.Requests.Validation;

namespace TrueCode.UserService;

public static class Extensions
{
    public static IServiceCollection AddUserServicesDependencies(this IServiceCollection collection,
        IConfiguration config)
    {
        collection.AddLinqToDBContext<UserContext>((provider, options) =>
            options.UsePostgreSQL(config.GetConnectionString("UserConnection") ??
                                  throw new ArgumentException("Cannot obtain connection string"))
                .UseDefaultLogging(provider));


        collection.AddScoped<IUserRepository, PostgresDbUserRepository>();
        collection.Configure<UserTokenCreationOptions>(config.GetSection("Token"));
        collection.AddSingleton<IAccessTokenCreator<User>, AccessTokenCreator>();
        collection
            .AddScoped<IAuthenticationService<UserLoginCommand, TryGetResult<string>>, UserAuthenticationService>();


        collection.AddScoped<IUserService, RepositoryUserService>();
        collection.AddSingleton<IHashCreator<string>, PasswordStringHashCreator>();


        collection.AddSingleton<IValidator<LogonUserRequest>, LogonUserRequestValidator>();
        collection.AddSingleton<IValidator<CreateUserRequest>, CreateUserRequestValidator>();


        return collection;
    }
}

public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}