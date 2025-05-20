using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Models.ModelBuilders;
public static class ProductModelBuilders
{
    public static void Build(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .Property(p => p.Unit)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Product>()
            .Property(p => p.VAT)
            .IsRequired()
            .HasPrecision(3, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.Discount)
            .IsRequired()
            .HasPrecision(3, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Product>()
            .Property(p => p.Origin)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .Property(p => p.Preservation)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .Property(p => p.Supplier)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Product>()
            .Property(p => p.Notes)
            .IsRequired()
            .HasMaxLength(500);

        modelBuilder.Entity<Product>()
            .Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        modelBuilder.Entity<Product>()
            .Property(p => p.Image)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ProductCategoryId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.SeoMeta)
            .WithOne(s => s.Product)
            .HasForeignKey<Product>(p => p.SeoMetaId)
            .IsRequired(false);
    }
}