using LinqToDB.Mapping;

namespace TrueCode.CurrencyFillerService.Infrastructure;

[Table("FillerServiceInfo")]
public class FillerServiceInfo
{
    [PrimaryKey] public int Id { get; init; } = 1;
    [Column] public DateTime LastUpdated { get; init; }
}