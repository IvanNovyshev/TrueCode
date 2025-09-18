namespace TrueCode.FinanceService;

public interface IFinanceService
{
    public Task<IEnumerable<Currency>> GetFavorites(string name);

    public Task SetFavorites(SetFavoritesCommand command);
}