using LinqToDB.Mapping;

namespace TrueCode.FinanceService;

[Table("FavoriteCurrencies")]
public class FavoritesDb
{
    [Column("Name")] public required string Name { get; init; }
    [Column("Code")] public required string Code { get; init; }
}