using Microsoft.EntityFrameworkCore;
using UMS.Features;

namespace UMS.Persistence;

public class UserDbContext: DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.EmailAddress)
            .IsUnique();
    }
}