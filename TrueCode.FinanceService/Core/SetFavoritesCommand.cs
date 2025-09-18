namespace TrueCode.FinanceService.Core;

public class SetFavoritesCommand
{
    public string Name { get; init; }
    public IReadOnlyCollection<string> Codes { get; init; }
}