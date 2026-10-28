using ArticleService.Data;
using ArticleService.DTOs;
using ArticleService.Models;
using ArticleService.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ArticleService.Tests;

public class ArticleServiceTests
{
    private static ArticleService.Services.ArticleService CreateService()
    {
        var options = new DbContextOptionsBuilder<ArticleDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new ArticleService.Services.ArticleService(new ArticleDbContext(options));
    }

    [Fact]
    public async Task CreateAsync_GeneratesSlugAndLimitsSeoFields()
    {
        var service = CreateService();
        var article = await service.CreateAsync(new SaveArticleRequest
        {
            Title = "A useful article", Content = "<p>Body</p>",
            MetaTitle = new string('t', 80), MetaDescription = new string('d', 200)
        }, CancellationToken.None);

        Assert.Equal("a-useful-article", article.Slug);
        Assert.Equal(60, article.MetaTitle.Length);
        Assert.Equal(160, article.MetaDescription.Length);
        Assert.Equal(ArticleStatus.Draft, article.Status);
    }

    [Fact]
    public async Task GetAllAsync_PublishedOnlyExcludesDrafts()
    {
        var service = CreateService();
        await service.CreateAsync(new SaveArticleRequest { Title = "Draft", Content = "draft", Status = ArticleStatus.Draft }, CancellationToken.None);
        await service.CreateAsync(new SaveArticleRequest { Title = "Live", Content = "live", Status = ArticleStatus.Published }, CancellationToken.None);

        var articles = await service.GetAllAsync(true, CancellationToken.None);

        var article = Assert.Single(articles);
        Assert.Equal("Live", article.Title);
        Assert.NotNull(article.PublishedAt);
    }

    [Fact]
    public async Task UpdateAsync_DraftClearsPublishedDate()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new SaveArticleRequest { Title = "Live", Content = "body", Status = ArticleStatus.Published }, CancellationToken.None);

        var updated = await service.UpdateAsync(created.Id, new SaveArticleRequest { Title = "Live", Content = "changed", Status = ArticleStatus.Draft }, CancellationToken.None);

        Assert.NotNull(updated);
        Assert.Equal("changed", updated.Content);
        Assert.Null(updated.PublishedAt);
    }
}
