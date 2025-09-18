using TrueCode.CurrencyFillerService.Core;

namespace TrueCode.CurrencyFillerService;

public class MainService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;


    public MainService(ILogger<MainService> logger, IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _factory.CreateScope())
            {
                var source = scope.ServiceProvider.GetRequiredService<ICurrencySource>();
                var storage = scope.ServiceProvider.GetRequiredService<ICurrencyStorage>();

                var toUpload = await source.GetNewValuesOrDefault(stoppingToken);

                if (toUpload != default)
                {
                    await storage.SaveAsync(toUpload, stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}