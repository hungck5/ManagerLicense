using ArticleService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Data;

public class ArticleDbContext(DbContextOptions<ArticleDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles => Set<Article>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(article => article.Id);
            entity.HasIndex(article => article.Slug).IsUnique();
            entity.Property(article => article.Title).HasMaxLength(200).IsRequired();
            entity.Property(article => article.Slug).HasMaxLength(220).IsRequired();
            entity.Property(article => article.Excerpt).HasMaxLength(500);
            entity.Property(article => article.MetaTitle).HasMaxLength(60);
            entity.Property(article => article.MetaDescription).HasMaxLength(160);
            entity.Property(article => article.CanonicalUrl).HasMaxLength(500);
            entity.Property(article => article.Author).HasMaxLength(100);
        });
    }
}
