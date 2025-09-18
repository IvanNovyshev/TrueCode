namespace TrueCode.FinanceService.Infrastructure;

public class SetFavoritesRequest
{
    public IReadOnlyCollection<string> Codes { get; init; } = [];
    public string Name { get; init; } = string.Empty;
}