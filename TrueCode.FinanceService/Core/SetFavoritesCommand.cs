namespace TrueCode.FinanceService.Core;

public class SetFavoritesCommand
{
    public required string Name { get; init; }
    public required IReadOnlyCollection<string> Codes { get; init; }
}