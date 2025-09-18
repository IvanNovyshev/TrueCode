using LinqToDB.Mapping;

namespace TrueCode.FinanceService.Infrastructure;

[Table("Currencies")]
public class CurrencyDb
{
    [PrimaryKey, Identity] public int Id { get; init; }
    [Column("Name")] public required string Name { get; init; }
    [Column("Rate")] public required decimal Rate { get; init; }
}