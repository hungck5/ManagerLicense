using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Models.ModelBuilders;

public static class SeoMetaModelBuilder
{
    public static void Build(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeoMeta>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.MetaDescription)
            .IsRequired()
            .HasMaxLength(160);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.Keywords)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.Author)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.Robots)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.CanonicalUrl)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.OgTitle)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.OgDescription)
            .IsRequired()
            .HasMaxLength(160);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.OgImage)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.OgUrl)
            .IsRequired()
            .HasMaxLength(250);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.Slug)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<SeoMeta>()
            .Property(s => s.ContentHtml)
            .IsRequired()
            .HasMaxLength(1000);

        modelBuilder.Entity<SeoMeta>()
            .HasOne(s => s.MenuItem)
            .WithOne(m => m.SeoMeta)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeoMeta>()
            .HasOne(s => s.ProductCategory)
            .WithOne(pc => pc.SeoMeta)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeoMeta>()
            .HasOne(s => s.Product)
            .WithOne(p => p.SeoMeta)
            .IsRequired(false);
    }
}
