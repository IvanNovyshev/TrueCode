using LinqToDB.Mapping;

namespace TrueCode.UserService.Infrastructure;

[Table("Users")]
public class UserDb
{
    [PrimaryKey, Identity] public int Id { get; init; }

    [Column("Name"), NotNull] public required string Name { get; init; }
    [Column("Hash"), NotNull] public required string Hash { get; init; }
}