using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.EfCore.Context;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<FeedBack> FeedBacks { get; set; }
    public DbSet<UserFeedBack> UserFeedBacks { get; set; }





    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=WIN-EFOC9MRP7RE;Database=EcommerceXetayi;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>()
            .Property(p => p.FeedBack)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>()
           .Property(p => p.TotalAmount)
           .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<FeedBack>()
           .Property(f => f.FeedBackMark)
           .HasColumnType("decimal(18,2)");
        //modelBuilder.Entity<UserFeedBack>()
        //    .HasOne(uf => uf.OrderItem)
        //    .WithMany(oi => oi.UserFeedBacks)
        //    .HasForeignKey(uf => uf.OrderItemId);

        modelBuilder.Entity<UserFeedBack>()
            .HasOne(uf => uf.FeedBack)
            .WithMany(fb => fb.UserFeedBacks)
            .HasForeignKey(uf => uf.FeedBackId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserFeedBack>()
            .HasOne(uf => uf.Product)
            .WithMany(p => p.UserFeedBacks)
            .HasForeignKey(uf => uf.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
