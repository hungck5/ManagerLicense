using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Models.ModelBuilders;
public static class MenuItemModelBuilder
{
  public static void Build(this ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<MenuItem>()
        .HasKey(m => m.Id);
        
    modelBuilder.Entity<MenuItem>()
        .Property(m => m.Id)
        .IsRequired()
        .HasMaxLength(100);

    modelBuilder.Entity<MenuItem>()
        .Property(m => m.Label)
        .IsRequired()
        .HasMaxLength(100);

    modelBuilder.Entity<MenuItem>()
        .Property(m => m.SeoMetaId)
        .IsRequired();

    modelBuilder.Entity<MenuItem>()
        .Property(m => m.ParentId)
        .IsRequired(false);

    modelBuilder.Entity<MenuItem>()
        .Property(m => m.Order)
        .IsRequired()
        .HasDefaultValue(0);

    modelBuilder.Entity<MenuItem>()
        .HasOne(m => m.SeoMeta)
        .WithOne(s => s.MenuItem)
        .HasForeignKey<MenuItem>(m => m.SeoMetaId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}