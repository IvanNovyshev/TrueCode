namespace TrueCode.FinanceService;

public class SetFavoritesCommand
{
    public string Name { get; init; }
    public IReadOnlyCollection<string> Codes { get; init; }
}