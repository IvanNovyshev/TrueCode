namespace TrueCode.FinanceService.Core;

public interface IFinanceService
{
    public Task<IEnumerable<Currency>> GetRatesForUserAsync(string name);

    public Task SetFavoritesCodesForUserAsync(SetFavoritesCommand command);
}