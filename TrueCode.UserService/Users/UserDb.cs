using System.ComponentModel.DataAnnotations;

namespace TrueCode.UserService.Users;

public class UserDb
{
    [Key] public int Id { get; init; }

    [MaxLength(1024)] public required string Name { get; init; }
    [MaxLength(1024)] public required string Hash { get; init; }
}