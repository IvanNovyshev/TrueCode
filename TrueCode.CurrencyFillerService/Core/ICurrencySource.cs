namespace TrueCode.CurrencyFillerService.Core;

public interface ICurrencySource
{
    Task<(DateTime, IEnumerable<Currency>)> GetNewValuesOrDefault(CancellationToken cancellationToken);
}