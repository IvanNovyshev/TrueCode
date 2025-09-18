namespace TrueCode.CurrencyFillerService.Core;

public interface ICurrencyStorage
{
    public Task SaveAsync((DateTime Date, IEnumerable<Currency> Currencies) source, CancellationToken token);
    public Task<DateTime> LastUpdated(CancellationToken token);
}