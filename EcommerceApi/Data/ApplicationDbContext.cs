using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

namespace EcommerceApi.Data;
public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : base(options) {}

  public DbSet<Product> Products => Set<Product>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
    modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
    modelBuilder.Entity<Product>().Property(p => p.VAT).HasPrecision(3, 2);
    modelBuilder.Entity<Product>().Property(p => p.Discount).HasPrecision(3, 2);
    modelBuilder.Entity<Product>().Property(p => p.CreatedDate).IsRequired();
    modelBuilder.Entity<Product>().Property(p => p.UpdatedDate).IsRequired();
    modelBuilder.Entity<Product>().Property(p => p.Status).HasMaxLength(50);
    modelBuilder.Entity<Product>().Property(p => p.Origin).HasMaxLength(100);
    modelBuilder.Entity<Product>().Property(p => p.Preservation).HasMaxLength(100);
    modelBuilder.Entity<Product>().Property(p => p.Supplier).HasMaxLength(100);
    modelBuilder.Entity<Product>().Property(p => p.Notes).HasMaxLength(500);
    modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(1000);
    modelBuilder.Entity<Product>().Property(p => p.Image).HasMaxLength(250);
    modelBuilder.Entity<Product>().Property(p => p.Category).HasMaxLength(100);
  }
}