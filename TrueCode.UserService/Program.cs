using TrueCode.UserService.Core;
using TrueCode.UserService.Requests;

namespace TrueCode.UserService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();

        builder.Services.AddOpenApi();

        if (builder.Environment.IsDevelopment())
        {
            // можно замокать репозитории если нужно
        }

        builder.Services.AddUserServicesDependencies(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("/user/logon", async (
            LogonUserRequest request,
            IAuthenticationService<UserLoginCommand, TryGetResult<string>> authService,
            ILogger<Program> logger) =>
        {
            try
            {
                var logonResult = await
                    authService.TryGetTokenAsync(new UserLoginCommand
                        { Name = request.Name, Password = request.Password });

                if (logonResult.IsSuccess)
                {
                    return Results.Ok(logonResult.Value);
                }

                return Results.Unauthorized();
            }
            catch (UserAuthenticationException e)
            {
                return Results.Problem();
            }
        }).WithValidation<LogonUserRequest>();


        app.MapPost("/user/create", async (
            CreateUserRequest request,
            IUserService userService,
            ILogger<Program> logger) =>
        {
            try
            {
                await userService.CreateUserAsync(new() { Name = request.Name, Password = request.Password });
                return Results.Created();
            }
            catch (UserAlreadyExistsException e)
            {
                logger.LogInformation("User with name {EName} already exists", e.Name);
                return Results.Conflict(new
                {
                    Title = "User already exists",
                    Detail = $"User with name {e.Name} already exists",
                });
            }
            catch
            {
                return Results.Problem();
            }
        }).WithValidation<CreateUserRequest>();

        app.Run();
    }
}