using LinqToDB.Mapping;

namespace TrueCode.FinanceService.Infrastructure;

[Table("FavoriteCurrencies")]
public class FavoritesDb
{
    [Column("Name"), NotNull] public required string Name { get; init; }
    [Column("Code"), NotNull] public required string Code { get; init; }
}