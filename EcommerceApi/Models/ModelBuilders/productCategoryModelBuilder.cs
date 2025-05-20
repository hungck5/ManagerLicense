using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Models.ModelBuilders;

public static class ProductCategoryModelBuilder
{
    public static void Build(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>()
            .HasKey(p => p.Id);
            
        modelBuilder.Entity<ProductCategory>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<ProductCategory>()
            .Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        modelBuilder.Entity<ProductCategory>()
            .Property(p => p.Image)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<ProductCategory>()
            .HasMany(p => p.Products)
            .WithOne(p => p.ProductCategory)
            .HasForeignKey(p => p.ProductCategoryId);
            
        modelBuilder.Entity<ProductCategory>()
            .HasOne(p => p.SeoMeta)
            .WithOne(p => p.ProductCategory)
            .HasForeignKey<ProductCategory>(p => p.SeoMetaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
