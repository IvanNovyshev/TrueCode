using Microsoft.EntityFrameworkCore;

namespace TrueCode.UserService.Users;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<UserDb> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDb>(builder => { builder.HasIndex(db => db.Name).IsUnique(); });

        base.OnModelCreating(modelBuilder);
    }
}