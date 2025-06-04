using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;
using EcommerceApi.Models.ModelBuilders;

namespace EcommerceApi.Db;
public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : base(options) {}

  public DbSet<Product> Products => Set<Product>();
  public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
  public DbSet<SeoMeta> SeoMetas => Set<SeoMeta>();
  public DbSet<MenuItem> MenuItems => Set<MenuItem>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    ProductModelBuilders.Build(modelBuilder);
    ProductCategoryModelBuilder.Build(modelBuilder);
    SeoMetaModelBuilder.Build(modelBuilder);
    MenuItemModelBuilder.Build(modelBuilder);
  }
}