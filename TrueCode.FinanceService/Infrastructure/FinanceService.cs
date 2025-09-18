using LinqToDB;
using LinqToDB.Data;
using TrueCode.FinanceService.Core;

namespace TrueCode.FinanceService.Infrastructure;

public class FinanceService : IFinanceService
{
    private readonly CurrencyContext _context;

    public FinanceService(CurrencyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Currency>> GetFavorites(string name)
    {
        var currencies = _context.Favorites.Where(x => x.Name == name);

        var test2 = await _context.Currencies.Where(x => currencies.Any(a => a.Code == x.Name)).ToArrayAsync();

        return test2.Select(x => new Currency { Name = x.Name, Rate = x.Rate });
    }

    public async Task SetFavorites(SetFavoritesCommand command)
    {
        await using var transaction = await _context.BeginTransactionAsync();

        try
        {
            await _context.Favorites.Where(x => x.Name == command.Name).DeleteAsync();

            await _context.Favorites.BulkCopyAsync(command.Codes.Select(x => new FavoritesDb()
                { Name = command.Name, Code = x }));

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
        }
    }
}