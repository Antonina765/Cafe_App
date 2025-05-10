using Cafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data;

public class WebDbContext : DbContext
{
    public const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=CafeApp;Username=postgres;Password=12345;";

    public DbSet<CafeData> Cafes { get; set; }
    public DbSet<UserData> Users { get; set; }
    public DbSet<ChatMessageData> ChatMessages { get; set; }
    public DbSet<MenuItemData> MenuItems { get; set; }
    public DbSet<BookingData> Bookings { get; set; }
    
    public WebDbContext(DbContextOptions<WebDbContext> contextOptions)
        : base(contextOptions) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(CONNECTION_STRING);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserData>()
            .HasMany(x => x.ChatMessages)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.SetNull);

    }
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserData>().HasKey(us => us.Id);

        modelBuilder.Entity<UserData>()
            .HasMany(p => p.Cafes)
            .WithOne(x => x.Creator)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(p => p.CreatorId);
    }*/
}