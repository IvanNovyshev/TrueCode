using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace TrueCode.MigrationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidAudience = builder.Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Token:Secret"] ??
                                               throw new ArgumentException("Cannot obtain jwt secret")))
                };
            });


        builder.Services.AddAuthorization();


        builder.Services.AddOpenApi();
        builder.Services.AddSingleton<IMigrationLock, SingletonMigrationLock>();
        builder.Services.AddScoped<IMigrationService, CommonMigrationService>();

        builder.Services
            .AddFluentMigratorCore()
            .ConfigureRunner(runnerBuilder =>
                runnerBuilder.AddPostgres().WithGlobalConnectionString("dbConnection").ScanIn(typeof(Program).Assembly)
                    .For
                    .Migrations()).AddLogging(lb => lb.AddFluentMigratorConsole());
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapPost("/v1/migrate/up", [Authorize(Roles = "MigrationAdmin")] async (IMigrationService ms) =>
        {
            try
            {
                await ms.MigrateUpAsync();
                return Results.Ok();
            }
            catch (MigrationException me)
            {
                return Results.Problem($"Migrate up failed. {me.Message}");
            }
            catch (Exception e)
            {
                // internal
                return Results.InternalServerError();
            }
        });

        app.MapPost("/v1/migrate/rollback/{step}", [Authorize(Roles = "MigrationAdmin")]
            async (int step, IMigrationService ms) =>
            {
                try
                {
                    await ms.RollbackAsync(step);
                    return Results.Ok();
                }
                catch (MigrationException me)
                {
                    return Results.Problem($"Rollback failed. {me.Message}");
                }
                catch (Exception e)
                {
                    // internal
                    return Results.InternalServerError();
                }
            });

        app.MapPost("/v1/migrate/down/{version}", [Authorize(Roles = "MigrationAdmin")]
            async (long version, IMigrationService ms) =>
            {
                try
                {
                    await ms.MigrateDownAsync(version);
                    return Results.Ok();
                }
                catch (MigrationException me)
                {
                    return Results.Problem($"Migrate down failed. {me.Message}");
                }
                catch (Exception e)
                {
                    // internal
                    return Results.InternalServerError();
                }
            });

        app.MapGet("/v1/migrations", [Authorize(Roles = "MigrationAdmin")] async (IMigrationService ms) =>
        {
            try
            {
                var migrations = await ms.GetMigrationsAsync();
                return Results.Ok(new MigrationsResponse { Migrations = migrations });
            }
            catch (MigrationException me)
            {
                return Results.Problem($"Get migrations failed. {me.Message}");
            }
            catch (Exception e)
            {
                // internal
                return Results.InternalServerError();
            }
        });

        app.MapGet("/health", () => Results.Ok());

        app.Run();
    }
}

public interface IMigrationService
{
    Task MigrateUpAsync();
    Task MigrateDownAsync(long version);
    Task<IReadOnlyCollection<string>> GetMigrationsAsync();
    Task RollbackAsync(int steps);
}

public class CommonMigrationService : IMigrationService
{
    private readonly IMigrationRunner _migrationRunner;
    private readonly IMigrationLock _migrationLock;
    private readonly ILogger<CommonMigrationService> _logger;


    public CommonMigrationService(IMigrationRunner migrationRunner, IMigrationLock migrationLock,
        ILogger<CommonMigrationService> logger)
    {
        _migrationRunner = migrationRunner;
        _migrationLock = migrationLock;
        _logger = logger;
    }

    public async Task MigrateUpAsync()
    {
        try
        {
            using var lck = await _migrationLock.LockAsync();
            _migrationRunner.MigrateUp();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Migrate Up failed.");
            throw new MigrationException(e.Message, e);
        }
    }

    public async Task MigrateDownAsync(long version)
    {
        try
        {
            using var lck = await _migrationLock.LockAsync();
            _migrationRunner.MigrateDown(version);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Migrate down failed.");
            throw new MigrationException(e.Message, e);
        }
    }

    public async Task<IReadOnlyCollection<string>> GetMigrationsAsync()
    {
        try
        {
            using var lck = await _migrationLock.LockAsync();
            var migrations = _migrationRunner.MigrationLoader.LoadMigrations();
            if (migrations != null)
            {
                return migrations.Select(x =>
                    $"{x.Key}|{x.Value.Description}|{x.Value.Version}|{x.Value.IsBreakingChange}").ToArray();
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Failed get all migrations");
            throw new MigrationException(e.Message, e);
        }


        return [];
    }

    public async Task RollbackAsync(int steps)
    {
        try
        {
            using var lck = await _migrationLock.LockAsync();
            _migrationRunner.Rollback(steps);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Rollback failed");
            throw new MigrationException(e.Message, e);
        }
    }
}

public class MigrationException : Exception
{
    public MigrationException(string message, Exception? exception = null) : base(message, exception)
    {
    }
}

public class MigrationsResponse
{
    public IReadOnlyCollection<string> Migrations { get; set; } = [];
}

public interface IMigrationLock
{
    public Task<IDisposable> LockAsync();
}

public class SingletonMigrationLock : IMigrationLock, IDisposable
{
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public async Task<IDisposable> LockAsync()
    {
        var lockResult = await _lock.WaitAsync(TimeSpan.FromSeconds(10)); // possible use IOptions

        if (!lockResult)
        {
            throw new MigrationException("Migration in progress. Please wait.");
        }

        return new SemaphoreWrapper(_lock);
    }


    public void Dispose()
    {
        _lock.Dispose();
    }

    private class SemaphoreWrapper : IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private bool _disposed = false;

        public SemaphoreWrapper(SemaphoreSlim semaphoreSlim)
        {
            _semaphoreSlim = semaphoreSlim;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _semaphoreSlim.Release();
            }

            _disposed = true;
        }
    }
}