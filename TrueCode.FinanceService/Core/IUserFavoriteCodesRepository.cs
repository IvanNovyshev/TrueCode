namespace TrueCode.FinanceService.Core;

public interface IUserFavoriteCodesRepository
{
    Task AddFavoriteCodesForUserAsync(string userName, IReadOnlyCollection<string> codes);
    
    Task RemoveFavoriteCodesByUserNameAsync(string userName);

    IQueryable<string> GetFavoriteCodes(string userName);

    public Task SaveAsync();
}