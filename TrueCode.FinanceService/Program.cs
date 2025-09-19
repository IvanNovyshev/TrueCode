using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using TrueCode.FinanceService.Core;
using TrueCode.FinanceService.Infrastructure;

namespace TrueCode.FinanceService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddLinqToDBContext<CurrencyContext>((provider, options) =>
            options.UseDefaultLogging(provider)
                .UsePostgreSQL(builder.Configuration.GetConnectionString("FinanceConnection") ??
                               throw new ArgumentException()));

        builder.Services.AddTransient<IUserFavoriteCodesRepository, UserFavoriteCodesRepository>();
        builder.Services.AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>();
        builder.Services.AddScoped<IFinanceService, MainFinanceService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/finances/favorites/{userName}",
            async (string userName, IFinanceService service) => await service.GetRatesForUserAsync(userName));

        app.MapPost("/finances/favorites",
            async (SetFavoritesRequest request, IFinanceService service) =>
                await service.SetFavoritesCodesForUserAsync(new SetFavoritesCommand { Name = request.Name, Codes = request.Codes }));

        app.Run();
    }
}