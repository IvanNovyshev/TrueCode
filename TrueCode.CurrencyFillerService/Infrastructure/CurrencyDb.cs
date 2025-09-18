using LinqToDB.Mapping;

namespace TrueCode.CurrencyFillerService.Infrastructure;

[Table("Currencies")]
public class CurrencyDb
{
    [Identity, PrimaryKey] public int Id { get; init; }

    [Column("Name")] public required string Name { get; init; }

    [Column("Rate")] public required decimal Rate { get; init; }
}