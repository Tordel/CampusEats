namespace CampusEats.Database;

using CampusEats.Features.Menu;
using CampusEats.Features.Orders;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
            entity.Property(e => e.SpecialInstructions).HasMaxLength(500);

            entity.OwnsMany(e => e.Items, item =>
            {
                item.Property(i => i.Price).HasPrecision(10, 2);
                item.Property(i => i.MenuItemName).IsRequired().HasMaxLength(200);
            });
        });
    }
}