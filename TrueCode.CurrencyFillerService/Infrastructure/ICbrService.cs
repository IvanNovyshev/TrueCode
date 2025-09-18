namespace TrueCode.CurrencyFillerService.Infrastructure;

public interface ICbrService
{
    Task<DateTime> AvailableDateAsync(CancellationToken token = default);

    Task<ValCurs> AllCurrenciesAsync(CancellationToken token = default);
}