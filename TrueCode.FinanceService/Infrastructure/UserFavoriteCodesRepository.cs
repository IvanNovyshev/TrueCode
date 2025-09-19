using System.Data;
using LinqToDB;
using LinqToDB.Data;
using TrueCode.FinanceService.Core;

namespace TrueCode.FinanceService.Infrastructure;

public class UserFavoriteCodesRepository : IUserFavoriteCodesRepository
{
    private readonly CurrencyContext _context;
    private readonly List<string> _removeList = new();
    private readonly List<(string UserName, IReadOnlyCollection<string> Codes)> _favoriteList = new();

    public UserFavoriteCodesRepository(CurrencyContext context)
    {
        _context = context;
    }

    public Task RemoveFavoriteCodesByUserNameAsync(string userName)
    {
        _removeList.Add(userName);
        return Task.CompletedTask;
    }

    public Task AddFavoriteCodesForUserAsync(string userName, IReadOnlyCollection<string> codes)
    {
        _favoriteList.Add((userName, codes));
        return Task.CompletedTask;
    }

    public IQueryable<string> GetFavoriteCodes(string userName)
    {
        return _context.Favorites.Where(x => x.Name == userName).Select(x => x.Code);
    }

    public async Task SaveAsync()
    {
        await using var transaction = await _context.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            await _context.Favorites.Where(x => _removeList.Any(c => c == x.Name)).DeleteAsync();

            var toInsert =
                _favoriteList.SelectMany(x => x.Codes.Select(c => new FavoritesDb { Name = x.UserName, Code = c }));

            await _context.Favorites.BulkCopyAsync(toInsert);

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            _removeList.Clear();
            _favoriteList.Clear();
        }
    }
}