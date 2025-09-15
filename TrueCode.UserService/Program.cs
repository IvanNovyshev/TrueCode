using Microsoft.EntityFrameworkCore;
using TrueCode.UserService.Authentication;
using TrueCode.UserService.Requests;
using TrueCode.UserService.Users;

namespace TrueCode.UserService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<UserContext>(options => options.UseInMemoryDatabase("JustName"));
        }

        builder.Services.AddUserServicesDependencies(builder.Configuration);
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserContext>();
            context.Database.EnsureCreated();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("/user/logon", async (
            LogonUserRequest request,
            IAuthenticationService<User, TryGetResult<string>> authService,
            ILogger<Program> logger) =>
        {
            var logonResult = await
                authService.TryGetTokenAsync(new User { Name = request.Name, Password = request.Password });

            if (logonResult.IsSuccess)
            {
                return Results.Ok(logonResult.Value);
            }

            return Results.Unauthorized();
        });

        app.MapPost("/user/create", async (
            CreateUserRequest request,
            IUserService userService,
            ILogger<Program> logger) =>
        {
            try
            {
                //TODO validate paayload
                await userService.CreateUserAsync(new User { Name = request.Name, Password = request.Password });
                return Results.Created();
            }
            catch (UserAlreadyExistsException e)
            {
                logger.LogInformation("User with name {EName} already exists", e.Name);
                return Results.Problem($"User with name {e.Name} already exists");
            }
            catch
            {
                //TODO
                return Results.Problem();
            }
        });

        app.Run();
    }
}