using LinqToDB;
using TrueCode.FinanceService.Core;

namespace TrueCode.FinanceService.Infrastructure;

public class CurrencyRatesRepository : ICurrencyRatesRepository
{
    private readonly CurrencyContext _context;

    public CurrencyRatesRepository(CurrencyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Currency>> GetRatesByCodes(IQueryable<string> codes)
    {
        return await _context.Currencies.Where(x => codes.Any(c => c == x.Name))
            .Select(x => new Currency() { Name = x.Name, Rate = x.Rate }).ToArrayAsync();
    }
}