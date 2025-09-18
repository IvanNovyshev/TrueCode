using TrueCode.CurrencyFillerService.Infrastructure;

namespace TrueCode.CurrencyFillerService.Core;

public class CbrCurrencySource : ICurrencySource
{
    private readonly ICurrencyStorage _storage;
    private readonly ICbrService _cbrService;

    public CbrCurrencySource(ICurrencyStorage storage, ICbrService cbrService)
    {
        _storage = storage;
        _cbrService = cbrService;
    }

    public async Task<(DateTime, IEnumerable<Currency>)> GetNewValuesOrDefault(CancellationToken cancellationToken)
    {
        var current = await _storage.LastUpdated(cancellationToken);
        var availableDate = await _cbrService.AvailableDateAsync(cancellationToken);

        if (DateOnly.FromDateTime(current) == DateOnly.FromDateTime(availableDate))
        {
            return default;
        }

        var allCurrencies = await _cbrService.AllCurrenciesAsync(cancellationToken);

        return (allCurrencies.Date, allCurrencies.Valutes.Select(x =>
                new Currency()
                    { Name = x.CharCode, Rate = x.Value })
            .ToArray()
            .AsReadOnly());
    }
}