namespace TrueCode.FinanceService.Core;

public interface IUserFavoriteCodesRepository
{
    Task AddFavoriteCodesForUser(string userName, IReadOnlyCollection<string> codes);
    
    Task RemoveFavoriteCodesByUserName(string userName);

    IQueryable<string> GetFavoriteCodes(string userName);

    public Task SaveAsync();
}