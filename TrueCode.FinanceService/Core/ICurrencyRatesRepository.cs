namespace TrueCode.FinanceService.Core;

public interface ICurrencyRatesRepository
{
    Task<IEnumerable<Currency>> GetRatesByCodes(IQueryable<string> names);
}