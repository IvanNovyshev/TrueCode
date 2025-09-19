using LinqToDB.Mapping;

namespace TrueCode.FinanceService.Infrastructure;

[Table("Currencies")]
public class CurrencyDb
{
    [PrimaryKey, Identity] public int Id { get; init; }
    [Column("Name"), NotNull] public required string Name { get; init; }
    [Column("Rate"), NotNull] public required decimal Rate { get; init; }
}