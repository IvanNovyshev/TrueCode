namespace TrueCode.UserService.Core;

public class User
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Password { get; init; }
    
}

public class NewUser
{
    public required string Name { get; init; }
    public required string Hash { get; init; }
    
}