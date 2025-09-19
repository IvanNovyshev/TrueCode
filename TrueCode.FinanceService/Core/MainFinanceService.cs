namespace TrueCode.FinanceService.Core;

public class MainFinanceService : IFinanceService
{
    private readonly IUserFavoriteCodesRepository _userFavoriteCodesRepository;
    private readonly ICurrencyRatesRepository _ratesRepository;

    public MainFinanceService(IUserFavoriteCodesRepository userFavoriteCodesRepository,
        ICurrencyRatesRepository ratesRepository)
    {
        _userFavoriteCodesRepository = userFavoriteCodesRepository;
        _ratesRepository = ratesRepository;
    }

    public async Task<IEnumerable<Currency>> GetRatesForUserAsync(string name)
    {
        try
        {
            var favoriteCodes = _userFavoriteCodesRepository.GetFavoriteCodes(name);
            var userRates = await _ratesRepository.GetRatesByCodes(favoriteCodes);

            return userRates;
        }
        catch (Exception e)
        {
            throw new FinanceServiceException(e.Message, e)
                { Description = $"Coundn't get favorites for user {name}" };
        }
    }

    public async Task SetFavoritesCodesForUserAsync(SetFavoritesCommand command)
    {
        try
        {
            await _userFavoriteCodesRepository.RemoveFavoriteCodesByUserNameAsync(command.Name);
            await _userFavoriteCodesRepository.AddFavoriteCodesForUserAsync(command.Name, command.Codes);

            await _userFavoriteCodesRepository.SaveAsync();
        }
        catch (Exception e)
        {
            throw new FinanceServiceException(e.Message, e)
                { Description = $"Coundn't set favorites for user {command.Name}" };
        }
    }
}

public class FinanceServiceException : Exception
{
    public FinanceServiceException(string message, Exception? inner = null) : base(message, inner)
    {
    }

    public required string Description { get; init; }
}