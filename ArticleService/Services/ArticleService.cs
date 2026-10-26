using ArticleService.Data;
using ArticleService.DTOs;
using ArticleService.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace ArticleService.Services;

public class ArticleService(ArticleDbContext context) : IArticleService
{
    public async Task<IReadOnlyList<ArticleDto>> GetAllAsync(bool publishedOnly, CancellationToken cancellationToken)
    {
        var query = context.Articles.AsNoTracking();
        if (publishedOnly) query = query.Where(article => article.Status == ArticleStatus.Published);
        return await query.OrderByDescending(article => article.PublishedAt ?? article.UpdatedAt)
            .Select(article => ToDto(article)).ToListAsync(cancellationToken);
    }

    public async Task<ArticleDto?> GetBySlugAsync(string slug, bool publishedOnly, CancellationToken cancellationToken)
    {
        var query = context.Articles.AsNoTracking().Where(article => article.Slug == slug);
        if (publishedOnly) query = query.Where(article => article.Status == ArticleStatus.Published);
        var article = await query.FirstOrDefaultAsync(cancellationToken);
        return article is null ? null : ToDto(article);
    }

    public async Task<ArticleDto> CreateAsync(SaveArticleRequest request, CancellationToken cancellationToken)
    {
        var article = new Article();
        Apply(article, request);
        context.Articles.Add(article);
        await context.SaveChangesAsync(cancellationToken);
        return ToDto(article);
    }

    public async Task<ArticleDto?> UpdateAsync(Guid id, SaveArticleRequest request, CancellationToken cancellationToken)
    {
        var article = await context.Articles.FindAsync([id], cancellationToken);
        if (article is null) return null;
        Apply(article, request);
        await context.SaveChangesAsync(cancellationToken);
        return ToDto(article);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var article = await context.Articles.FindAsync([id], cancellationToken);
        if (article is null) return false;
        context.Articles.Remove(article);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static void Apply(Article article, SaveArticleRequest request)
    {
        var slug = Slugify(string.IsNullOrWhiteSpace(request.Slug) ? request.Title : request.Slug);
        article.Title = request.Title.Trim();
        article.Slug = slug;
        article.Excerpt = request.Excerpt?.Trim() ?? string.Empty;
        article.Content = request.Content.Trim();
        article.MetaTitle = (request.MetaTitle?.Trim() ?? article.Title)[..Math.Min((request.MetaTitle?.Trim() ?? article.Title).Length, 60)];
        article.MetaDescription = (request.MetaDescription?.Trim() ?? article.Excerpt)[..Math.Min((request.MetaDescription?.Trim() ?? article.Excerpt).Length, 160)];
        article.CanonicalUrl = request.CanonicalUrl?.Trim() ?? string.Empty;
        article.Author = request.Author?.Trim() ?? string.Empty;
        article.Status = request.Status;
        article.UpdatedAt = DateTime.UtcNow;
        article.PublishedAt = request.Status == ArticleStatus.Published ? article.PublishedAt ?? DateTime.UtcNow : null;
    }

    private static string Slugify(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();
        foreach (var character in normalized.Where(character => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character) != System.Globalization.UnicodeCategory.NonSpacingMark))
            builder.Append(character == 'đ' ? 'd' : character == 'Đ' ? 'D' : character);
        var slug = Regex.Replace(builder.ToString().ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');
        return string.IsNullOrEmpty(slug) ? Guid.NewGuid().ToString("N") : slug;
    }

    private static ArticleDto ToDto(Article article) => new()
    {
        Id = article.Id, Title = article.Title, Slug = article.Slug, Excerpt = article.Excerpt,
        Content = article.Content, MetaTitle = article.MetaTitle, MetaDescription = article.MetaDescription,
        CanonicalUrl = article.CanonicalUrl, Author = article.Author, Status = article.Status,
        CreatedAt = article.CreatedAt, UpdatedAt = article.UpdatedAt, PublishedAt = article.PublishedAt
    };
}
